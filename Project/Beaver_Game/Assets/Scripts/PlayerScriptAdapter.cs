using Photon.Pun;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScriptAdapter : MonoBehaviourPunCallbacks
{
    private MapImages mapImages;
    private ItemEquipManager itemEquipManager;
    private PutDownItem putDownItem;
    private ThrowAwayItem throwAwayItem;
    private InventorySlotGroup inventorySlotGroup;
    private GameWinManager gameWinManager;
    private NetworkManager networkManager;
    private SoundEffectManager soundEffectManager;


    void Start()
    {
        if (!this.gameObject.GetPhotonView().IsMine)
        {
            return;
        }

        mapImages = GameObject.Find("MapImagePieces").GetComponent<MapImages>();
        mapImages.player = this.gameObject;
        itemEquipManager = GameObject.Find("EquipSlots").GetComponent<ItemEquipManager>();
        itemEquipManager.player = this.gameObject;
        putDownItem = GameObject.Find("PutDownSolt").GetComponent<PutDownItem>();
        putDownItem.playerPos = this.gameObject.transform;
        throwAwayItem = GameObject.Find("ThrowAwaySlot").GetComponent<ThrowAwayItem>();
        throwAwayItem.prisonManager = this.gameObject.GetComponent<PrisonManager>();

        inventorySlotGroup = GameObject.Find("InventorySlots").GetComponent<InventorySlotGroup>();
        inventorySlotGroup.spyBoolManager = this.gameObject.GetComponent<SpyBoolManager>();
        gameWinManager = GameObject.Find("GameOverManager").GetComponent<GameWinManager>();
        gameWinManager.spyBoolManager = this.gameObject.GetComponent<SpyBoolManager>();
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        networkManager.player = this.gameObject;

        soundEffectManager = GameObject.Find("SoundEffectManager").GetComponent<SoundEffectManager>();
        soundEffectManager.playerAudioSource = GetComponent<AudioSource>();
        soundEffectManager.SetVolume();
    }
}
