using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace ParticleEditor.Helpers.UndoRedo
{
    public class UndoManager : ObservableObject
    {
        private int _limit = 50;

        private static readonly UndoManager _instance = new UndoManager();
        public static UndoManager Instance
        {
            get { return _instance; }
        }

        public LinkedList<IUndoRedo> UndoList { get; set; } = new LinkedList<IUndoRedo>();
        public LinkedList<IUndoRedo> RedoList { get; set; } = new LinkedList<IUndoRedo>();

        public void Add<T>(T instance) where T : IUndoRedo
        {
            UndoList.AddFirst(instance);
            RedoList.Clear();

            while (UndoList.Count > _limit)
                UndoList.RemoveLast();

            RaisePropertyChanged("UndoDescription");
        }

        public RelayCommand UndoCommand => new RelayCommand(Undo, ()=>UndoList.Count > 0);
        public void Undo()
        {
            if (UndoList.Count > 0)
            {
                IUndoRedo item = UndoList.First();
                UndoList.RemoveFirst();

                LinkedList<IUndoRedo> cpyRedo = new LinkedList<IUndoRedo>(RedoList.ToList());
                cpyRedo.AddFirst(item);
                LinkedList<IUndoRedo> cpyUndo = new LinkedList<IUndoRedo>(UndoList.ToList());
                item.Undo();

                RedoList = new LinkedList<IUndoRedo>(cpyRedo);
                UndoList = new LinkedList<IUndoRedo>(cpyUndo);
                RaisePropertyChanged("RedoDescription");
                RaisePropertyChanged("UndoDescription");
            }
        }

        public RelayCommand RedoCommand => new RelayCommand(Redo, ()=> RedoList.Count > 0);
        public void Redo()
        {
            if (RedoList.Count > 0)
            {
                IUndoRedo item = RedoList.First();
                RedoList.RemoveFirst();

                LinkedList<IUndoRedo> cpyRedo = new LinkedList<IUndoRedo>(RedoList.ToList());
                item.Redo();

                RedoList = new LinkedList<IUndoRedo>(cpyRedo);
                RaisePropertyChanged("RedoDescription");
                RaisePropertyChanged("UndoDescription");
            }
        }

        public string UndoDescription
        {
            get
            {
                if (UndoList.Count == 0) return "Undo";
                return $"Undo {UndoList.First.Value.Description}";
            }
        }

        public string RedoDescription
        {
            get
            {
                if (RedoList.Count == 0) return "Redo";
                return $"Redo {RedoList.First.Value.Description}";
            }
        }
    }
}
