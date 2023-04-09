using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using SpaceWarp;
using SpaceWarp.API.Mods;
using SpaceWarp.API.Mods.JSON;
using UnityEngine;
using UnityEngine.UIElements;

public class ModListController
{
    // UXML template for list entries
    private VisualTreeAsset ListEntryTemplate;

    // Mod list UI element references
    private ListView SpaceWarpModList;
    private ListView UnmanagedInfoModList;
    private ListView UnmanagedModList;
    private ListView DisabledInfoModList;
    private ListView DisabledModList;

    // Details UI element references
    private Label DetailsNameLabel;
    private Label DetailsIdLabel;
    private Label DetailsAuthorLabel;
    private Label DetailsVersionLabel;
    private Label DetailsSourceLabel;
    private Label DetailsDescriptionLabel;
    private Label DetailsKspVersionLabel;
    private VisualElement DetailsContainer;
    private ListView DetailsDependenciesList;

    // State
    
    public void InitializeLists(VisualElement root, VisualTreeAsset listElementTemplate)
    {
        //EnumerateAllCharacters();

        // Store a reference to the template for the list entries
        ListEntryTemplate = listElementTemplate;

        // Store references to the Mod list UI elements
        SpaceWarpModList = root.Q<ListView>("spacewarp-mod-list");
        UnmanagedInfoModList = root.Q<ListView>("unmanaged-info-mod-list");
        UnmanagedModList = root.Q<ListView>("unmanaged-mod-list");
        DisabledInfoModList = root.Q<ListView>("disabled-info-mod-list");
        DisabledModList = root.Q<ListView>("disabled-mod-list");

        // Store references to the selected mod details UI element references
        DetailsNameLabel = root.Q<Label>("details-name");
        DetailsIdLabel = root.Q<Label>("details-id");
        DetailsAuthorLabel = root.Q<Label>("details-author");
        DetailsVersionLabel = root.Q<Label>("details-version");
        DetailsSourceLabel = root.Q<Label>("details-source");
        DetailsDescriptionLabel = root.Q<Label>("details-description");
        DetailsKspVersionLabel = root.Q<Label>("details-ksp-version");
        DetailsContainer = root.Q<VisualElement>("details-container");
        DetailsDependenciesList = root.Q<ListView>("details-dependencies-list");

        FillModLists();

        // Register to get a callback when a mod is selected
        SpaceWarpModList.onSelectionChange += OnModSelected(SpaceWarpModList);
        UnmanagedInfoModList.onSelectionChange += OnModSelected(UnmanagedInfoModList);
        UnmanagedModList.onSelectionChange += OnModSelected(UnmanagedModList);
        DisabledInfoModList.onSelectionChange += OnModSelected(DisabledInfoModList);
        DisabledModList.onSelectionChange += OnModSelected(DisabledModList);
    }

    // private void EnumerateAllCharacters()
    // {
    //     AllCharacters = new List<CharacterData>();
    //     AllCharacters.AddRange(Resources.LoadAll<CharacterData>("Characters"));
    // }

    private void FillModLists()
    {
        // Set up a make item function for a list entry
        VisualElement MakeItem()
        {
            Debug.Log($"Making item");
            var newListEntry = ListEntryTemplate.Instantiate();

            var newListEntryLogic = new ModListItemController();
            newListEntry.userData = newListEntryLogic; 
            newListEntryLogic.SetVisualElement(newListEntry);

            return newListEntry;
        }

        SpaceWarpModList.makeItem = MakeItem;
        UnmanagedInfoModList.makeItem = MakeItem;
        UnmanagedModList.makeItem = MakeItem;
        DisabledInfoModList.makeItem = MakeItem;
        DisabledModList.makeItem = MakeItem;

        // Set up bind function for a specific list entry
        SpaceWarpModList.bindItem = (item, index) =>
        {
            Debug.Log($"Binding SpaceWarpModList item");
            var listItem = (ModListItemController)item.userData;
            var info = SpaceWarpManager.SpaceWarpPlugins[index].SpaceWarpMetadata;
            
            listItem.SetModInfo(info);
            
            if (SpaceWarpManager.ModsOutdated[info.ModID])
            {
                listItem.SetIsOutdated(true);
            }
            if (SpaceWarpManager.ModsUnsupported[info.ModID])
            {
                listItem.SetIsOutdated(true);
            }
        };
        
        UnmanagedInfoModList.bindItem = (item, index) =>
        {
            var listItem = (ModListItemController)item.userData;
            var info = SpaceWarpManager.NonSpaceWarpInfos[index].Item2;
            
            listItem.SetModInfo(info);
            
            if (SpaceWarpManager.ModsOutdated[info.ModID])
            {
                listItem.SetIsOutdated(true);
            }
            if (SpaceWarpManager.ModsUnsupported[info.ModID])
            {
                listItem.SetIsOutdated(true);
            }
        };
        UnmanagedModList.bindItem = (item, index) =>
        {
            var listItem = (ModListItemController)item.userData;
            var info = SpaceWarpManager.NonSpaceWarpPlugins[index].Info;
            
            listItem.SetPluginInfo(info);
        };

        DisabledInfoModList.bindItem = (item, index) =>
        {
            var listItem = (ModListItemController)item.userData;
            var info = SpaceWarpManager.DisabledInfoPlugins[index].Item2;
            
            listItem.SetModInfo(info);
            listItem.SetIsDisabled(true);
        };
        DisabledModList.bindItem = (item, index) =>
        {
            var listItem = (ModListItemController)item.userData;
            var info = SpaceWarpManager.DisabledNonInfoPlugins[index];
            
            listItem.SetPluginInfo(info);
            listItem.SetIsDisabled(true);
        };

        // Set the actual item's source list/array
        SpaceWarpModList.itemsSource = SpaceWarpManager.SpaceWarpPlugins.ToList();
        UnmanagedInfoModList.itemsSource = SpaceWarpManager.NonSpaceWarpInfos.ToList();
        UnmanagedModList.itemsSource = SpaceWarpManager.NonSpaceWarpPlugins.ToList();
        DisabledInfoModList.itemsSource = SpaceWarpManager.DisabledInfoPlugins.ToList();
        DisabledModList.itemsSource = SpaceWarpManager.DisabledNonInfoPlugins.ToList();
    }

    private Action<IEnumerable<object>> OnModSelected(ListView listView) => _ =>
    {
        var selectedItem = listView.selectedItem;

        switch (selectedItem)
        {
            // Handle none-selection (Escape to deselect everything)
            case null:
                ClearSelected();
                return;
            
            case BaseSpaceWarpPlugin plugin:
                SetSelectedModInfo(plugin.SpaceWarpMetadata);
                return;
            
            case Tuple<BaseUnityPlugin, ModInfo> plugin:
                SetSelectedModInfo(plugin.Item2);
                return;
            
            case BaseUnityPlugin plugin:
                SetSelectedPluginInfo(plugin.Info);
                return;
            
            case Tuple<PluginInfo, ModInfo> plugin:
                SetSelectedModInfo(plugin.Item2);
                return;
            
            case PluginInfo plugin:
                SetSelectedPluginInfo(plugin);
                return;
        }
    };

    private void SetSelectedModInfo(ModInfo info)
    {
        SetSelected(
            info.Name,
            info.ModID,
            info.Author,
            info.Version,
            info.Source,
            info.Description,
            info.SupportedKsp2Versions.ToString(),
            info.Dependencies
                .Select(dependencyInfo => (dependencyInfo.ID, dependencyInfo.Version.ToString()))
                .ToList()
        );
    }
    
    private void SetSelectedPluginInfo(PluginInfo info)
    {
        SetSelected(info.Metadata.Name, info.Metadata.GUID, version: info.Metadata.Version.ToString());
    }

    private void SetSelected(
        string name = "",
        string id = "",
        string author = "",
        string version = "",
        string source = "",
        string description = "",
        string kspVersion = "",
        List<(string, string)> dependencies = null,
        bool hasSelected = true
    )
    {
        DetailsNameLabel.text = name;
        DetailsIdLabel.text = id;
        DetailsAuthorLabel.text = author;
        DetailsVersionLabel.text = version;
        DetailsSourceLabel.text = source;
        DetailsDescriptionLabel.text = description;
        DetailsKspVersionLabel.text = kspVersion;
        DetailsDependenciesList.itemsSource = dependencies;
        DetailsContainer.visible = hasSelected;
    }

    private void ClearSelected()
    {
        SetSelected(hasSelected: false);
    }
}