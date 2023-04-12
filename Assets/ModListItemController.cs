using BepInEx;
using SpaceWarp.API.Mods.JSON;
using UnityEngine.UIElements;

public class ModListItemController
{
    private VisualElement _element;
    private Label _nameLabel;

    public void SetVisualElement(VisualElement visualElement)
    {
        _element = visualElement;
        _element.userData = this;
        _nameLabel = _element.Q<Label>(className: "mod-item-label");
    }

    public string Guid;

    public object Info;

    public void SetModInfo(ModInfo info)
    {
        Info = info;
        _nameLabel.text = info.Name;
    }

    public void SetPluginInfo(PluginInfo info)
    {
        Info = info;
        _nameLabel.text = info.Metadata.Name;
    }

    public void SetIsOutdated()
    {
        _nameLabel.AddToClassList("outdated");
    }

    public void SetIsUnsupported()
    {
        _nameLabel.AddToClassList("unsupported");
    }

    public void SetIsDisabled()
    {
        _nameLabel.AddToClassList("disabled");
    }
}