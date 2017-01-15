namespace ParticleEditor.Helpers
{
    public struct MessageData
    {
        public MessageData(MessageId id, object data = null)
        {
            Id = id;
            Data = data;
        }

        public object Data;
        public MessageId Id;
    }

    public enum MessageId
    {
        ParticleSystemChanged,
        ImageChanged,
        BurstChanged,
        ColorChanged,
        KeyframeSelected,
    }
}
