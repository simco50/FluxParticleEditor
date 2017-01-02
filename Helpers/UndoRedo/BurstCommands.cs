using DrWPF.Windows.Data;
using GalaSoft.MvvmLight.Messaging;
using ParticleEditor.Model.Data;
using ParticleEditor.ViewModels;
using ParticleEditor.ViewModels.ParameterTabs;

namespace ParticleEditor.Helpers.UndoRedo
{
    public class BurstAddCommand : IUndoRedo
    {
        private Burst burst;
        private ParticleSystem instance;
        public BurstAddCommand(ParticleSystem instance, Burst b)
        {
            this.instance = instance;
            burst = b;
            Description = "Add burst";
        }

        public string Description { get; }
        public void Undo()
        {
            var list = (ObservableSortedDictionary<float, int>)instance.GetType().GetProperty("Bursts").GetValue(instance);
            list.Remove(burst.Time);
            Messenger.Default.Send<MessageData, ParticleVisualizerViewModel>(new MessageData(MessageID.BurstChanged));
        }

        public void Redo()
        {
            var list = (ObservableSortedDictionary<float, int>)instance.GetType().GetProperty("Bursts").GetValue(instance);
            list[burst.Time] = burst.Amount;
            UndoManager.Instance.Add(new BurstAddCommand(instance, burst));
            Messenger.Default.Send<MessageData, ParticleVisualizerViewModel>(new MessageData(MessageID.BurstChanged));
        }
    }

    public class BurstRemoveCommand : IUndoRedo
    {
        private Burst burst;
        private ParticleSystem instance;
        public BurstRemoveCommand(ParticleSystem instance, Burst b)
        {
            this.instance = instance;
            burst = b;
            Description = "Remove burst";
        }

        public string Description { get; }
        public void Undo()
        {
            var list = (ObservableSortedDictionary<float, int>)instance.GetType().GetProperty("Bursts").GetValue(instance);
            list[burst.Time] = burst.Amount;
            Messenger.Default.Send<MessageData, ParticleVisualizerViewModel>(new MessageData(MessageID.BurstChanged));
        }

        public void Redo()
        {
            var list = (ObservableSortedDictionary<float, int>)instance.GetType().GetProperty("Bursts").GetValue(instance);
            list.Remove(burst.Time);
            UndoManager.Instance.Add(new BurstRemoveCommand(instance, burst));
            Messenger.Default.Send<MessageData, ParticleVisualizerViewModel>(new MessageData(MessageID.BurstChanged));
        }
    }
}
