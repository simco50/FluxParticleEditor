using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using ParticleEditor.Helpers;
using ParticleEditor.Helpers.UndoRedo;
using ParticleEditor.Model.Data;

namespace ParticleEditor.ViewModels.ParameterTabs
{
    public class Burst
    {
        public float Time = -1;
        public int Amount = -1;

        public bool IsValid()
        {
            return !(Amount < 0 || Time < 0);
        }
    }

    public class BurstTabViewModel : ViewModelBase
    {
        public ParticleSystem ParticleSystem { get; set; }

        public RelayCommand<Burst> AddBurstCommand => new RelayCommand<Burst>(AddBurst);
        private void AddBurst(Burst burst)
        {
            if (burst.IsValid() == false)
            {
                DebugLog.Log("Input format not valid! Make sure the values are not negative and only contain numbers.", "Particle System", LogSeverity.Warning);
                return;
            }
            if (burst.Time > ParticleSystem.Duration)
            {
                DebugLog.Log($"The time must be smaller than the duration of the particle system.\nThe duration is {ParticleSystem.Duration:0.00}s while the given timestamp is {burst.Time:0.00}s.", "Particle System", LogSeverity.Warning);
                return;
            }
            UndoManager.Instance.Add(new BurstAddCommand(ParticleSystem, burst));

            ParticleSystem.Bursts[burst.Time] = burst.Amount;

            Messenger.Default.Send<MessageData, ParticleVisualizerViewModel>(new MessageData(MessageId.BurstChanged));
        }

        public RelayCommand<float> RemoveBurstCommand => new RelayCommand<float>(RemoveBurst);
        private void RemoveBurst(float key)
        {
            UndoManager.Instance.Add(new BurstRemoveCommand(ParticleSystem, new Burst {Amount = ParticleSystem.Bursts[key], Time = key}));

            if (ParticleSystem.Bursts.Remove(key) == false)
                DebugLog.Log($"Key with value {key} does not exists!", "Particle System", LogSeverity.Warning);

            Messenger.Default.Send<MessageData, ParticleVisualizerViewModel>(new MessageData(MessageId.BurstChanged));
        }
    }
}
