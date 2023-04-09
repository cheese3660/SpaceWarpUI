using BepInEx;
using SpaceWarp.API.Mods.JSON;
using UnityEngine.UIElements;

public class ModListItemController
{
    private Label _nameLabel;

    public ModInfo _modInfo;
    public PluginInfo _pluginInfo;
    public bool _isOutdated;
    public bool _isUnsupported;
    public bool _isDisabled;

    public void SetVisualElement(VisualElement visualElement)
    {
        _nameLabel = visualElement.Q<Label>("mod-item-label");
    }

    public string ModGuid;
    public void SetData(ModListItemData data)
    {
        ModGuid = data.GUID;
        _nameLabel.text = data.Name;
    }

    public void SetModInfo(ModInfo info)
    {
        _modInfo = info;
        _pluginInfo = null;
        _nameLabel.text = info.Name;
    }

    public void SetPluginInfo(PluginInfo info)
    {
        _pluginInfo = info;
        _modInfo = null;
        _nameLabel.text = info.Metadata.Name;
    }

    public void SetIsOutdated(bool isOutdated)
    {
        _isOutdated = isOutdated;
        _nameLabel.AddToClassList("outdated");
    }

    public void SetIsUnsupported(bool isUnsupported)
    {
        _isUnsupported = isUnsupported;
        _nameLabel.AddToClassList("unsupported");
    }

    public void SetIsDisabled(bool isDisabled)
    {
        _isDisabled = isDisabled;
        _nameLabel.AddToClassList("disabled");
    }
}