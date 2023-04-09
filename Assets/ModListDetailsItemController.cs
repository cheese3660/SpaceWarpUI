using UnityEngine.UIElements;

public class ModListDetailsItemController
{
    private Label _nameLabel;
    private Label _versionLabel;

    public void SetVisualElement(VisualElement visualElement)
    {
        _nameLabel = visualElement.Q<Label>(className: "details-key-label");
        _versionLabel = visualElement.Q<Label>("details-value-label");
    }

    public void SetInfo(string name, string version)
    {
        _nameLabel.text = name;
        _versionLabel.text = version;
    }
}