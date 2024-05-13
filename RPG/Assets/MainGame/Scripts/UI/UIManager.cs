using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GI {
    public class UIManager : MonoBehaviour
    {
        public PlayerManager player;
        public ItemStatsWindowUI itemStatsWindowUI;
        public EquipmentWindowUI equipmentWindowUI;
        public QuickSlotsUI quickSlotsUI;

        [Header("HUD")]
        public GameObject crossHair;
        public Text xpCount;

        [Header("UI Windows")]
        public GameObject hudWindow;
        public GameObject selectWindow;
        public GameObject equipmentScreenWindow;
        public GameObject weaponInventoryWindow;
        public GameObject headEquipmentInventoryWindow;
        public GameObject bodyEquipmentInventoryWindow;
        public GameObject legEquipmentInventoryWindow;
        public GameObject handEquipmentInventoryWindow;
        public GameObject ringInventoryWindow;
        public GameObject spellInventoryWindow;
        public GameObject consumableInventoryWindow;
        public GameObject ammoInventoryWindow;
        public GameObject itemStatsWindow;
        public GameObject levelUpWindow;
        public GameObject menuOptionsWindow;

        [Header("Vendor UI Windows")]
        public GameObject weaponsVendorShopWindow;
        public GameObject armorVendorShopWindow;
        public GameObject mageVendorShopWindow;

        [Header("Equipment Window Slot Selected")]
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool rightHandSlot03Selected;
        public bool rightHandSlot04Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;
        public bool leftHandSlot03Selected;
        public bool leftHandSlot04Selected;
        public bool headEquipmentSlotSelected;
        public bool bodyEquipmentSlotSelected;
        public bool legEquipmentSlotSelected;
        public bool handEquipmentSlotSelected;
        public bool ringItemSlot01Selected;
        public bool ringItemSlot02Selected;
        public bool ringItemSlot03Selected;
        public bool ringItemSlot04Selected;
        public bool spellSlotSelected;
        public bool consumableSlotSelected;
        public bool ammoSlotSelected;

        [Header("Pop Ups")]
        BonfireLitPopUpUI bonfireLitPopUpUI;

        [Header("Weapon Inventory")]
        public GameObject weaponInventorySlotPrefab;
        public Transform weaponInventorySlotsParent;
        WeaponInventorySlot[] weaponInventorySlots;

        [Header("Head Equipment Inventory")]
        public GameObject headEquipmentInventorySlotPrefab;
        public Transform headEquipmentInventorySlotParent;
        HeadEquipmentInventorySlot[] headEquipmentInventorySlots;

        [Header("Body Equipment Inventory")]
        public GameObject bodyEquipmentInventorySlotPrefab;
        public Transform bodyEquipmentInventorySlotParent;
        BodyEquipmentInventorySlot[] bodyEquipmentInventorySlots;

        [Header("Leg Equipment Inventory")]
        public GameObject legEquipmentInventorySlotPrefab;
        public Transform legEquipmentInventorySlotParent;
        LegEquipmentInventorySlot[] legEquipmentInventorySlots;

        [Header("Hand Equipment Inventory")]
        public GameObject handEquipmentInventorySlotPrefab;
        public Transform handEquipmentInventorySlotParent;
        HandEquipmentInventorySlot[] handEquipmentInventorySlots;

        [Header("Ring Items Inventory")]
        public GameObject ringItemInventorySlotPrefab;
        public Transform ringItemInventorySlotParent;
        RingItemInventorySlot[] ringItemInventorySlots;

        [Header("Spell Inventory")]
        public GameObject spellInventorySlotPrefab;
        public Transform spellInventorySlotParent;
        SpellItemInventorySlot[] spellInventorySlots;

        [Header("Consumable Inventory")]
        public GameObject consumableInventorySlotPrefab;
        public Transform consumableInventorySlotParent;
        ConsumableInventorySlot[] consumableInventorySlots;

        [Header("Ammo Inventory")]
        public GameObject ammoInventorySlotPrefab;
        public Transform ammoInventorySlotParent;
        AmmoInventorySlot[] ammoInventorySlots;

        [Header("Confirmation Pop Ups")]
        public GameObject confirmPurchaseWindow;
        public GameObject notEnoughMoneyWindow;

        [Header("Vendor's Weapon Inventory")]
        public GameObject confirmWeaponPurchaseWindow;
        public WeaponsVendorInventoryManager vendorWeaponInventoryManager;
        public GameObject vendorWeaponInventorySlotPrefab;
        public Transform vendorWeaponInventorySlotsParent;
        WeaponInventorySlot[] vendorWeaponInventorySlots;
        public WeaponItem vendorSelectedWeapon;

        [Header("Vendor's Ammo Inventory")]
        public GameObject confirmAmmoPurchaseWindow;
        public GameObject vendorAmmoInventorySlotPrefab;
        public Transform vendorAmmoInventorySlotsParent;
        AmmoInventorySlot[] vendorAmmoInventorySlots;
        public RangedAmmoItem vendorSelectedAmmo;

        [Header("Armor Vendor Inventory")]
        public ArmorVendorInventoryManager armorVendorInventoryManager;

        [Header("Head Armor Vendor Inventory")]
        public GameObject confirmHelmetPurchaseWindow;
        public GameObject helmetVendorInventorySlotPrefab;
        public Transform helmetVendorInventorySlotParent;
        HeadEquipmentInventorySlot[] vendorHeadEquipmentInventorySlots;
        public HelmetEquipment vendorSelectedHelmet;

        [Header("Body Armor Vendor Inventory")]
        public GameObject confirmBodyPurchaseWindow;
        public GameObject bodyVendorInventorySlotPrefab;
        public Transform bodyVendorInventorySlotParent;
        BodyEquipmentInventorySlot[] vendorBodyEquipmentInventorySlots;
        public BodyEquipment vendorSelectedBody;

        [Header("Hand Armor Vendor Inventory")]
        public GameObject confirmHandPurchaseWindow;
        public GameObject handVendorInventorySlotPrefab;
        public Transform handVendorInventorySlotParent;
        HandEquipmentInventorySlot[] vendorHandEquipmentInventorySlots;
        public HandEquipment vendorSelectedHand;

        [Header("Leg Armor Vendor Inventory")]
        public GameObject confirmLegPurchaseWindow;
        public GameObject legVendorInventorySlotPrefab;
        public Transform legVendorInventorySlotParent;
        LegEquipmentInventorySlot[] vendorLegEquipmentInventorySlots;
        public LegEquipment vendorSelectedLeg;

        [Header("Mage Vendor Inventory")]
        public MageVendorInventoryManager mageVendorInventoryManager;

        [Header("Spell Mage Vendor Inventory")]
        public GameObject confirmSpellPurchaseWindow;
        public GameObject spellVendorInventorySlotPrefab;
        public Transform spellVendorInventorySlotParent;
        SpellItemInventorySlot[] vendorSpellInventorySlots;
        public SpellItem vendorSelectedSpell;

        [Header("Consumable Mage Vendor Inventory")]
        public GameObject confirmConsumablePurchaseWindow;
        public GameObject consumableVendorInventorySlotPrefab;
        public Transform consumableVendorInventorySlotParent;
        ConsumableInventorySlot[] vendorConsumableInventorySlots;
        public ConsumableItem vendorSelectedConsumable;

        [Header("Ring Mage Vendor Inventory")]
        public GameObject confirmRingPurchaseWindow;
        public GameObject ringVendorInventorySlotPrefab;
        public Transform ringVendorInventorySlotParent;
        RingItemInventorySlot[] vendorRingInventorySlots;
        public RingItem vendorSelectedRing;

        private void Awake()
        {
            quickSlotsUI = GetComponentInChildren<QuickSlotsUI>();
            player = FindObjectOfType<PlayerManager>();

            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            headEquipmentInventorySlots = headEquipmentInventorySlotParent.GetComponentsInChildren<HeadEquipmentInventorySlot>();
            bodyEquipmentInventorySlots = bodyEquipmentInventorySlotParent.GetComponentsInChildren<BodyEquipmentInventorySlot>();
            legEquipmentInventorySlots = legEquipmentInventorySlotParent.GetComponentsInChildren<LegEquipmentInventorySlot>();
            handEquipmentInventorySlots = handEquipmentInventorySlotParent.GetComponentsInChildren<HandEquipmentInventorySlot>();
            ringItemInventorySlots = ringItemInventorySlotParent.GetComponentsInChildren<RingItemInventorySlot>();
            spellInventorySlots = spellInventorySlotParent.GetComponentsInChildren<SpellItemInventorySlot>();
            consumableInventorySlots = consumableInventorySlotParent.GetComponentsInChildren<ConsumableInventorySlot>();
            ammoInventorySlots = ammoInventorySlotParent.GetComponentsInChildren<AmmoInventorySlot>();

            vendorWeaponInventorySlots = vendorWeaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            vendorAmmoInventorySlots = vendorAmmoInventorySlotsParent.GetComponentsInChildren<AmmoInventorySlot>();
            vendorHeadEquipmentInventorySlots = helmetVendorInventorySlotParent.GetComponentsInChildren<HeadEquipmentInventorySlot>();
            vendorBodyEquipmentInventorySlots = bodyVendorInventorySlotParent.GetComponentsInChildren<BodyEquipmentInventorySlot>();
            vendorHandEquipmentInventorySlots = handVendorInventorySlotParent.GetComponentsInChildren<HandEquipmentInventorySlot>();
            vendorLegEquipmentInventorySlots = legVendorInventorySlotParent.GetComponentsInChildren<LegEquipmentInventorySlot>();
            vendorSpellInventorySlots = spellVendorInventorySlotParent.GetComponentsInChildren<SpellItemInventorySlot>();
            vendorConsumableInventorySlots = consumableVendorInventorySlotParent.GetComponentsInChildren<ConsumableInventorySlot>();
            vendorRingInventorySlots = ringVendorInventorySlotParent.GetComponentsInChildren<RingItemInventorySlot>();

            bonfireLitPopUpUI = GetComponentInChildren<BonfireLitPopUpUI>();
        }

        private void Start()
        {
            equipmentWindowUI.LoadWeaponOnEquipmentScreen(player);
            equipmentWindowUI.LoadArmorOnEquipmentScreen(player);
            equipmentWindowUI.LoadRingItemOnEquipmentScreen(player);
            equipmentWindowUI.LoadSpellOnEquipmentScreen(player);
            equipmentWindowUI.LoadConsumableOnEquipmentScreen(player);
            equipmentWindowUI.LoadAmmoOnEquipmentScreen(player);

            if(player.playerInventoryManager.currentSpell != null)
            {
                quickSlotsUI.UpdateCurrentSpellIcon(player.playerInventoryManager.currentSpell);
            }

            if (player.playerInventoryManager.currentConsumable != null)
            {
                quickSlotsUI.UpdateCurrentConsumableIcon(player.playerInventoryManager.currentConsumable);
            }

            xpCount.text = player.playerStatsManager.currentXPCount.ToString();
        }

        public void UpdateUI()
        {
            UpdateInventorySlots();
            //Weapon Inventory Slots
            for (int i = 0; i < weaponInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.weaponsInventory.Count)
                {
                    if (weaponInventorySlots.Length < player.playerInventoryManager.weaponsInventory.Count)
                    {
                        Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    weaponInventorySlots[i].AddItem(player.playerInventoryManager.weaponsInventory[i]);
                }
                else
                {
                    weaponInventorySlots[i].ClearInventorySlot();                    
                }
            }
        
            //Head Equipment Inventory Slots

            for(int i = 0; i < headEquipmentInventorySlots.Length; i++)
            {
                if(i < player.playerInventoryManager.headEquipmentInventory.Count)
                {
                    if(headEquipmentInventorySlots.Length < player.playerInventoryManager.headEquipmentInventory.Count)
                    {
                        Instantiate(headEquipmentInventorySlotPrefab, headEquipmentInventorySlotParent);
                        headEquipmentInventorySlots = headEquipmentInventorySlotParent.GetComponentsInChildren<HeadEquipmentInventorySlot>();
                    }
                    headEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.headEquipmentInventory[i]);
                }
                else
                {
                    headEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }

            //Body Equipment Inventory Slots

            for (int i = 0; i < bodyEquipmentInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.bodyEquipmentInventory.Count)
                {
                    if (bodyEquipmentInventorySlots.Length < player.playerInventoryManager.bodyEquipmentInventory.Count)
                    {
                        Instantiate(bodyEquipmentInventorySlotPrefab, bodyEquipmentInventorySlotParent);
                        bodyEquipmentInventorySlots = bodyEquipmentInventorySlotParent.GetComponentsInChildren<BodyEquipmentInventorySlot>();
                    }
                    bodyEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.bodyEquipmentInventory[i]);
                }
                else
                {
                    bodyEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }

            //Leg Equipment Inventory Slots

            for (int i = 0; i < legEquipmentInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.legEquipmentInventory.Count)
                {
                    if (legEquipmentInventorySlots.Length < player.playerInventoryManager.legEquipmentInventory.Count)
                    {
                        Instantiate(legEquipmentInventorySlotPrefab, legEquipmentInventorySlotParent);
                        legEquipmentInventorySlots = legEquipmentInventorySlotParent.GetComponentsInChildren<LegEquipmentInventorySlot>();
                    }
                    legEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.legEquipmentInventory[i]);
                }
                else
                {
                    legEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }

            //Hand Equipment Inventory Slots

            for (int i = 0; i < handEquipmentInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.handEquipmentInventory.Count)
                {
                    if (handEquipmentInventorySlots.Length < player.playerInventoryManager.handEquipmentInventory.Count)
                    {
                        Instantiate(handEquipmentInventorySlotPrefab, handEquipmentInventorySlotParent);
                        handEquipmentInventorySlots = handEquipmentInventorySlotParent.GetComponentsInChildren<HandEquipmentInventorySlot>();
                    }
                    handEquipmentInventorySlots[i].AddItem(player.playerInventoryManager.handEquipmentInventory[i]);
                }
                else
                {
                    handEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }

            //Ring Item Inventory Slots

            for (int i = 0; i < ringItemInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.ringItemInventory.Count)
                {
                    if (ringItemInventorySlots.Length < player.playerInventoryManager.ringItemInventory.Count)
                    {
                        Instantiate(ringItemInventorySlotPrefab, ringItemInventorySlotParent);
                        ringItemInventorySlots = ringItemInventorySlotParent.GetComponentsInChildren<RingItemInventorySlot>();
                    }
                    ringItemInventorySlots[i].AddItem(player.playerInventoryManager.ringItemInventory[i]);
                }
                else
                {
                    ringItemInventorySlots[i].ClearInventorySlot();
                }
            }

            //Spell Inventory Slots

            for (int i = 0; i < spellInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.spellInventory.Count)
                {
                    if (spellInventorySlots.Length < player.playerInventoryManager.spellInventory.Count)
                    {
                        Instantiate(spellInventorySlotPrefab, spellInventorySlotParent);
                        spellInventorySlots = spellInventorySlotParent.GetComponentsInChildren<SpellItemInventorySlot>();
                    }
                    spellInventorySlots[i].AddItem(player.playerInventoryManager.spellInventory[i]);
                }
                else
                {
                    spellInventorySlots[i].ClearInventorySlot();
                }
            }

            //Consumable Inventory Slots

            for (int i = 0; i < consumableInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.consumableInventory.Count)
                {
                    if (consumableInventorySlots.Length < player.playerInventoryManager.consumableInventory.Count)
                    {
                        Instantiate(consumableInventorySlotPrefab, consumableInventorySlotParent);
                        consumableInventorySlots = consumableInventorySlotParent.GetComponentsInChildren<ConsumableInventorySlot>();
                    }
                    consumableInventorySlots[i].AddItem(player.playerInventoryManager.consumableInventory[i]);
                }
                else
                {
                    consumableInventorySlots[i].ClearInventorySlot();
                }
            }

            //Ammo Inventory Slots

            for (int i = 0; i < ammoInventorySlots.Length; i++)
            {
                if (i < player.playerInventoryManager.ammoInventory.Count)
                {
                    if (ammoInventorySlots.Length < player.playerInventoryManager.ammoInventory.Count)
                    {
                        Instantiate(ammoInventorySlotPrefab, ammoInventorySlotParent);
                        ammoInventorySlots = ammoInventorySlotParent.GetComponentsInChildren<AmmoInventorySlot>();
                    }
                    if (player.playerInventoryManager != null)
                    {
                        ammoInventorySlots[i].AddItem(player.playerInventoryManager.ammoInventory[i]);
                    }
                }
                else
                {
                    ammoInventorySlots[i].ClearInventorySlot();
                }
            }

            //Junk Inventory Slots


        }

        public void UpdateVendorUI()
        {
            UpdateVendorInventorySlots();

            for (int i = 0; i < vendorWeaponInventorySlots.Length; i++)
            {
                if (i < vendorWeaponInventoryManager.weaponsInventory.Count)
                {
                    if (vendorWeaponInventorySlots.Length < vendorWeaponInventoryManager.weaponsInventory.Count)
                    {
                        Instantiate(vendorWeaponInventorySlotPrefab, vendorWeaponInventorySlotsParent);
                        vendorWeaponInventorySlots = vendorWeaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                    }
                    vendorWeaponInventorySlots[i].AddItem(vendorWeaponInventoryManager.weaponsInventory[i]);
                }
                else
                {
                    vendorWeaponInventorySlots[i].ClearInventorySlot();
                }
            }

            for (int i = 0; i < vendorAmmoInventorySlots.Length; i++)
            {
                if (i < vendorWeaponInventoryManager.ammoInventory.Count)
                {
                    if (vendorAmmoInventorySlots.Length < vendorWeaponInventoryManager.ammoInventory.Count)
                    {
                        Instantiate(vendorAmmoInventorySlotPrefab, vendorAmmoInventorySlotsParent);
                        vendorAmmoInventorySlots = vendorAmmoInventorySlotsParent.GetComponentsInChildren<AmmoInventorySlot>();
                    }
                    vendorAmmoInventorySlots[i].AddItem(vendorWeaponInventoryManager.ammoInventory[i]);
                }
                else
                {
                    vendorAmmoInventorySlots[i].ClearInventorySlot();
                }
            }

            for (int i = 0; i < vendorHeadEquipmentInventorySlots.Length; i++)
            {
                if (i < armorVendorInventoryManager.headEquipmentInventory.Count)
                {
                    if (vendorHeadEquipmentInventorySlots.Length < armorVendorInventoryManager.headEquipmentInventory.Count)
                    {
                        Instantiate(helmetVendorInventorySlotPrefab, helmetVendorInventorySlotParent);
                        vendorHeadEquipmentInventorySlots = helmetVendorInventorySlotParent.GetComponentsInChildren<HeadEquipmentInventorySlot>();
                    }
                    vendorHeadEquipmentInventorySlots[i].AddItem(armorVendorInventoryManager.headEquipmentInventory[i]);
                }
                else
                {
                    vendorHeadEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }

            for (int i = 0; i < vendorBodyEquipmentInventorySlots.Length; i++)
            {
                if (i < armorVendorInventoryManager.bodyEquipmentInventory.Count)
                {
                    if (vendorBodyEquipmentInventorySlots.Length < armorVendorInventoryManager.bodyEquipmentInventory.Count)
                    {
                        Instantiate(bodyVendorInventorySlotPrefab, bodyVendorInventorySlotParent);
                        vendorBodyEquipmentInventorySlots = bodyVendorInventorySlotParent.GetComponentsInChildren<BodyEquipmentInventorySlot>();
                    }
                    vendorBodyEquipmentInventorySlots[i].AddItem(armorVendorInventoryManager.bodyEquipmentInventory[i]);
                }
                else
                {
                    vendorBodyEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }

            for (int i = 0; i < vendorHandEquipmentInventorySlots.Length; i++)
            {
                if (i < armorVendorInventoryManager.handEquipmentInventory.Count)
                {
                    if (vendorHandEquipmentInventorySlots.Length < armorVendorInventoryManager.handEquipmentInventory.Count)
                    {
                        Instantiate(handVendorInventorySlotPrefab, handVendorInventorySlotParent);
                        vendorHandEquipmentInventorySlots = handVendorInventorySlotParent.GetComponentsInChildren<HandEquipmentInventorySlot>();
                    }
                    vendorHandEquipmentInventorySlots[i].AddItem(armorVendorInventoryManager.handEquipmentInventory[i]);
                }
                else
                {
                    vendorHandEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }

            for (int i = 0; i < vendorLegEquipmentInventorySlots.Length; i++)
            {
                if (i < armorVendorInventoryManager.legEquipmentInventory.Count)
                {
                    if (vendorLegEquipmentInventorySlots.Length < armorVendorInventoryManager.legEquipmentInventory.Count)
                    {
                        Instantiate(legVendorInventorySlotPrefab, legVendorInventorySlotParent);
                        vendorLegEquipmentInventorySlots = legVendorInventorySlotParent.GetComponentsInChildren<LegEquipmentInventorySlot>();
                    }
                    vendorLegEquipmentInventorySlots[i].AddItem(armorVendorInventoryManager.legEquipmentInventory[i]);
                }
                else
                {
                    vendorLegEquipmentInventorySlots[i].ClearInventorySlot();
                }
            }

            for (int i = 0; i < vendorSpellInventorySlots.Length; i++)
            {
                if (i < mageVendorInventoryManager.spellInventory.Count)
                {
                    if (vendorSpellInventorySlots.Length < mageVendorInventoryManager.spellInventory.Count)
                    {
                        Instantiate(spellVendorInventorySlotPrefab, spellVendorInventorySlotParent);
                        vendorSpellInventorySlots = spellVendorInventorySlotParent.GetComponentsInChildren<SpellItemInventorySlot>();
                    }
                    vendorSpellInventorySlots[i].AddItem(mageVendorInventoryManager.spellInventory[i]);
                }
                else
                {
                    vendorSpellInventorySlots[i].ClearInventorySlot();
                }
            }

            for (int i = 0; i < vendorConsumableInventorySlots.Length; i++)
            {
                if (i < mageVendorInventoryManager.consumableInventory.Count)
                {
                    if (vendorConsumableInventorySlots.Length < mageVendorInventoryManager.consumableInventory.Count)
                    {
                        Instantiate(consumableVendorInventorySlotPrefab, consumableVendorInventorySlotParent);
                        vendorConsumableInventorySlots = consumableVendorInventorySlotParent.GetComponentsInChildren<ConsumableInventorySlot>();
                    }
                    vendorConsumableInventorySlots[i].AddItem(mageVendorInventoryManager.consumableInventory[i]);
                }
                else
                {
                    vendorConsumableInventorySlots[i].ClearInventorySlot();
                }
            }

            for (int i = 0; i < vendorRingInventorySlots.Length; i++)
            {
                if (i < mageVendorInventoryManager.ringInventory.Count)
                {
                    if (vendorRingInventorySlots.Length < mageVendorInventoryManager.ringInventory.Count)
                    {
                        Instantiate(ringVendorInventorySlotPrefab, ringVendorInventorySlotParent);
                        vendorRingInventorySlots = ringVendorInventorySlotParent.GetComponentsInChildren<RingItemInventorySlot>();
                    }
                    vendorRingInventorySlots[i].AddItem(mageVendorInventoryManager.ringInventory[i]);
                }
                else
                {
                    vendorRingInventorySlots[i].ClearInventorySlot();
                }
            }
        }

        public void UpdateInventorySlots()
        {
            weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            headEquipmentInventorySlots = headEquipmentInventorySlotParent.GetComponentsInChildren<HeadEquipmentInventorySlot>();
            bodyEquipmentInventorySlots = bodyEquipmentInventorySlotParent.GetComponentsInChildren<BodyEquipmentInventorySlot>();
            legEquipmentInventorySlots = legEquipmentInventorySlotParent.GetComponentsInChildren<LegEquipmentInventorySlot>();
            handEquipmentInventorySlots = handEquipmentInventorySlotParent.GetComponentsInChildren<HandEquipmentInventorySlot>();
            ringItemInventorySlots = ringItemInventorySlotParent.GetComponentsInChildren<RingItemInventorySlot>();
            spellInventorySlots = spellInventorySlotParent.GetComponentsInChildren<SpellItemInventorySlot>();
            consumableInventorySlots = consumableInventorySlotParent.GetComponentsInChildren<ConsumableInventorySlot>();
            ammoInventorySlots = ammoInventorySlotParent.GetComponentsInChildren<AmmoInventorySlot>();
        }

        public void UpdateVendorInventorySlots()
        {
            vendorWeaponInventorySlots = vendorWeaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
            vendorAmmoInventorySlots = vendorAmmoInventorySlotsParent.GetComponentsInChildren<AmmoInventorySlot>();
            vendorHeadEquipmentInventorySlots = helmetVendorInventorySlotParent.GetComponentsInChildren<HeadEquipmentInventorySlot>();
            vendorBodyEquipmentInventorySlots = bodyVendorInventorySlotParent.GetComponentsInChildren<BodyEquipmentInventorySlot>();
            vendorHandEquipmentInventorySlots = handVendorInventorySlotParent.GetComponentsInChildren<HandEquipmentInventorySlot>();
            vendorLegEquipmentInventorySlots = legVendorInventorySlotParent.GetComponentsInChildren<LegEquipmentInventorySlot>();
            vendorSpellInventorySlots = spellVendorInventorySlotParent.GetComponentsInChildren<SpellItemInventorySlot>();
            vendorConsumableInventorySlots = consumableVendorInventorySlotParent.GetComponentsInChildren<ConsumableInventorySlot>();
            vendorRingInventorySlots = ringVendorInventorySlotParent.GetComponentsInChildren<RingItemInventorySlot>();
        }

        public void OpenSelectWindow()
        {
            selectWindow.SetActive(true);
        }

        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
        }

        public void CloseAllInventoryWindows()
        {
            ResetAllSelectedSlots();
            weaponInventoryWindow.SetActive(false);
            equipmentScreenWindow.SetActive(false);
            headEquipmentInventoryWindow.SetActive(false);
            bodyEquipmentInventoryWindow.SetActive(false);
            legEquipmentInventoryWindow.SetActive(false);
            handEquipmentInventoryWindow.SetActive(false);
            ringInventoryWindow.SetActive(false);
            spellInventoryWindow.SetActive(false);
            consumableInventoryWindow.SetActive(false);
            ammoInventoryWindow.SetActive(false);            
            
            itemStatsWindow.SetActive(false);

            levelUpWindow.SetActive(false);
            weaponsVendorShopWindow.SetActive(false);
            armorVendorShopWindow.SetActive(false);
            mageVendorShopWindow.SetActive(false);
            menuOptionsWindow.SetActive(false);

            notEnoughMoneyWindow.SetActive(false);
            confirmPurchaseWindow.SetActive(false);
            confirmWeaponPurchaseWindow.SetActive(false);
            confirmHelmetPurchaseWindow.SetActive(false);
            confirmBodyPurchaseWindow.SetActive(false);
            confirmHandPurchaseWindow.SetActive(false);
            confirmLegPurchaseWindow.SetActive(false);


        }

        public void CloseAllVendorsConfirmWindows()
        {
            confirmHandPurchaseWindow.SetActive(false);
            confirmHelmetPurchaseWindow.SetActive(false);
            confirmBodyPurchaseWindow.SetActive(false);
            confirmLegPurchaseWindow.SetActive(false);
            confirmWeaponPurchaseWindow.SetActive(false);
            confirmAmmoPurchaseWindow.SetActive(false);
            confirmSpellPurchaseWindow.SetActive(false);
            confirmConsumablePurchaseWindow.SetActive(false);
            confirmRingPurchaseWindow.SetActive(false);
        }

        public void ResetAllSelectedSlots()
        {
            rightHandSlot01Selected = false;
            rightHandSlot02Selected = false;
            rightHandSlot03Selected = false;
            rightHandSlot04Selected = false;
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = false;
            leftHandSlot03Selected = false;
            leftHandSlot04Selected = false;

            headEquipmentSlotSelected = false;
            bodyEquipmentSlotSelected = false;
            legEquipmentSlotSelected = false;
            handEquipmentSlotSelected = false;

            ringItemSlot01Selected = false;
            ringItemSlot02Selected = false;
            ringItemSlot03Selected = false;
            ringItemSlot04Selected = false;

            spellSlotSelected = false;
            consumableSlotSelected = false;
            ammoSlotSelected = false;
        }

        public void ActivateBonfirePopUp()
        {
            bonfireLitPopUpUI.DisplayBonfireLitPopUp();
        }
    }
}
