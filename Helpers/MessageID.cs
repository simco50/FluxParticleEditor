namespace ParticleEditor.Helpers
{
    public struct MessageData
    {
        public MessageData(MessageID id, object data = null)
        {
            Id = id;
            Data = data;
        }

        public object Data;
        public MessageID Id;
    }

    public enum MessageID
    {
        ParticleSystemChanged,
        ImageChanged,
        BurstChanged,
        ColorChanged,
    }
}
