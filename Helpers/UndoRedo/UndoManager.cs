using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GalaSoft.MvvmLight.CommandWpf;

namespace ParticleEditor.Helpers.UndoRedo
{
    public static class UndoManager
    {
        private static int _limit = 50;

        public static LinkedList<IUndoRedo> UndoList { get; set; } = new LinkedList<IUndoRedo>();
        public static LinkedList<IUndoRedo> RedoList { get; set; } = new LinkedList<IUndoRedo>();

        public static void Add<T>(T instance) where T : IUndoRedo
        {
            UndoList.AddFirst(instance);
            RedoList.Clear();

            while (UndoList.Count > _limit)
                UndoList.RemoveLast();
        }

        public static RelayCommand UndoCommand => new RelayCommand(Undo, ()=>UndoList.Count > 0);
        public static void Undo()
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
            }
        }

        public static RelayCommand RedoCommand => new RelayCommand(Redo, ()=> RedoList.Count > 0);
        public static void Redo()
        {
            if (RedoList.Count > 0)
            {
                IUndoRedo item = RedoList.First();
                RedoList.RemoveFirst();

                LinkedList<IUndoRedo> cpyRedo = new LinkedList<IUndoRedo>(RedoList.ToList());
                item.Redo();

                RedoList = new LinkedList<IUndoRedo>(cpyRedo);
            }
        }
    }
}
