public interface ISelectable
{
    bool IsSelected { get; }

    bool Select();
    bool Deselect();
}
