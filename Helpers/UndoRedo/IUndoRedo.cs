using System;

namespace ParticleEditor.Helpers.UndoRedo
{
    public interface IUndoRedo
    {
        string Description { get; }
        void Undo();
        void Redo();
    }
}
