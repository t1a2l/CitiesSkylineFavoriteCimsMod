using System;
using System.Collections.Generic;
using ColossalFramework;
using ColossalFramework.UI;
using FavoriteCims.UI.PanelsRows;
using FavoriteCims.Utils;
using UnityEngine;

namespace FavoriteCims.UI.Panels
{
    public class PeopleInsideBuildingsPanel : UIPanel
	{
        private float seconds = 0.5f;

        private bool execute = false;

        private bool firstRun = true;

        public static bool Wait = false;

        private bool Garbage = false;

        public InstanceID BuildingID;

        public UIPanel RefPanel;

        private readonly BuildingManager MyBuilding = Singleton<BuildingManager>.instance;

        private readonly CitizenManager MyCitizen = Singleton<CitizenManager>.instance;

        public static Dictionary<uint, uint> CimsOnBuilding = [];

        public static int WorkersCount = 0;

        public static int GuestsCount = 0;

        private BuildingInfo buildingInfo;

        const int MaxHouseHolds = 26;
        const int MaxWorkersUnit = 20; // **Important** *same of MaxGuestsUnit*
        const int MaxGuestsUnit = 20;

        private readonly UIPanel[] ResidentialPanels = new UIPanel[MaxHouseHolds];

        private readonly UIPanel[] ResidentialPanelSubRow = new UIPanel[MaxHouseHolds];

        private readonly UIButton[] ResidentialPanelIcon = new UIButton[MaxHouseHolds];

        private readonly UIButton[] ResidentialPanelText = new UIButton[MaxHouseHolds];

        private readonly ResidentialBuildingPanelRow[] ResidentialBodyRow = new ResidentialBuildingPanelRow[MaxHouseHolds*5];

        private UIPanel WorkersPanel;

        private UIPanel WorkersPanelSubRow;

        private UIButton WorkersPanelIcon;

        private UIButton WorkersPanelText;

        private readonly WorkersBuildingPanelRow[] WorkersBodyRow = new WorkersBuildingPanelRow[100];

        private UIPanel GuestsPanel;

        private UIPanel GuestsPanelSubRow;

        private UIButton GuestsPanelIcon;

        private UIButton GuestsPanelText;

        private readonly GuestsBuildingPanelRow[] GuestsBodyRow = new GuestsBuildingPanelRow[100];

        private uint BuildingUnits;

        private UIPanel Title;

        private UITextureSprite TitleSpriteBg;

        private UIButton TitleBuildingName;

        private UIPanel Body;

        private UITextureSprite BodySpriteBg;

        private UIScrollablePanel BodyRows;

        private UIPanel Footer;

        private UITextureSprite FooterSpriteBg;

        private UIScrollablePanel BodyPanelScrollBar;

        private UIScrollbar BodyScrollBar;

        private UISlicedSprite BodyPanelTrackSprite;

        private UISlicedSprite thumbSprite;

        public override void Start()
		{
			try
			{
				width = 250f;
				height = 0f;
				name = "FavCimsPeopleInsideBuildingsPanel";
				absolutePosition = new Vector3(0f, 0f);
				Hide();
				Title = AddUIComponent<UIPanel>();
				Title.name = "PeopleInsideBuildingsPanelTitle";
				Title.width = width;
				Title.height = 41f;
				Title.relativePosition = Vector3.zero;
				TitleSpriteBg = Title.AddUIComponent<UITextureSprite>();
				TitleSpriteBg.name = "PeopleInsideBuildingsPanelTitleBG";
				TitleSpriteBg.width = Title.width;
				TitleSpriteBg.height = Title.height;
				TitleSpriteBg.texture = TextureDB.VehiclePanelTitleBackground;
				TitleSpriteBg.relativePosition = Vector3.zero;
				TitleBuildingName = Title.AddUIComponent<UIButton>();
				TitleBuildingName.name = "PeopleInsideBuildingsPanelName";
				TitleBuildingName.width = Title.width;
				TitleBuildingName.height = Title.height;
				TitleBuildingName.textVerticalAlignment = UIVerticalAlignment.Middle;
				TitleBuildingName.textHorizontalAlignment = UIHorizontalAlignment.Center;
				TitleBuildingName.playAudioEvents = false;
				TitleBuildingName.font = UIDynamicFont.FindByName("OpenSans-Regular");
				TitleBuildingName.font.size = 15;
				TitleBuildingName.textScale = 1f;
				TitleBuildingName.wordWrap = true;
				TitleBuildingName.textPadding.left = 5;
				TitleBuildingName.textPadding.right = 5;
				TitleBuildingName.textColor = new Color32(204, 204, 51, 40);
				TitleBuildingName.hoveredTextColor = new Color32(204, 204, 51, 40);
				TitleBuildingName.pressedTextColor = new Color32(204, 204, 51, 40);
				TitleBuildingName.focusedTextColor = new Color32(204, 204, 51, 40);
				TitleBuildingName.useDropShadow = true;
				TitleBuildingName.dropShadowOffset = new Vector2(1f, -1f);
				TitleBuildingName.dropShadowColor = new Color32(0, 0, 0, 0);
				TitleBuildingName.relativePosition = Vector3.zero;
				Body = AddUIComponent<UIPanel>();
				Body.name = "PeopleInsideBuildingsBody";
				Body.width = width;
				Body.autoLayoutDirection = LayoutDirection.Vertical;
				Body.autoLayout = true;
				Body.clipChildren = true;
				Body.height = 0f;
				Body.relativePosition = new Vector3(0f, Title.height);
				BodySpriteBg = Body.AddUIComponent<UITextureSprite>();
				BodySpriteBg.name = "PeopleInsideBuildingsDataContainer";
				BodySpriteBg.width = Body.width;
				BodySpriteBg.height = Body.height;
				BodySpriteBg.texture = TextureDB.VehiclePanelBackground;
				BodySpriteBg.relativePosition = Vector3.zero;
				BodyRows = BodySpriteBg.AddUIComponent<UIScrollablePanel>();
				BodyRows.name = "BodyRows";
				BodyRows.width = BodySpriteBg.width - 24f;
				BodyRows.autoLayoutDirection = LayoutDirection.Vertical;
				BodyRows.autoLayout = true;
				BodyRows.relativePosition = new Vector3(12f, 0f);
				string[] array = ["Residential", "Workers", "Guests"];
				for (int i = 0; i < 3; i++)
				{
					if (i == 0)
					{
						int row = 0;
						for (int j = 0; j < 26; j++)
						{
							ResidentialPanels[j] = BodyRows.AddUIComponent<UIPanel>();
							ResidentialPanels[j].width = 226f;
							ResidentialPanels[j].height = 25f;
							ResidentialPanels[j].name = "LabelPanel_" + array[i] + "_" + j.ToString();
							ResidentialPanels[j].autoLayoutDirection = LayoutDirection.Vertical;
							ResidentialPanels[j].autoLayout = true;
							ResidentialPanels[j].Hide();
							ResidentialPanelSubRow[j] = ResidentialPanels[j].AddUIComponent<UIPanel>();
							ResidentialPanelSubRow[j].width = 226f;
							ResidentialPanelSubRow[j].height = 25f;
							ResidentialPanelSubRow[j].name = "TitlePanel_" + array[i] + "_" + j.ToString();
							ResidentialPanelSubRow[j].atlas = MyAtlas.FavCimsAtlas;
							ResidentialPanelSubRow[j].backgroundSprite = "bg_row2";
							ResidentialPanelIcon[j] = ResidentialPanelSubRow[j].AddUIComponent<UIButton>();
							ResidentialPanelIcon[j].name = "LabelPanelIcon_" + array[i] + "_" + j.ToString();
							ResidentialPanelIcon[j].width = 17f;
							ResidentialPanelIcon[j].height = 17f;
							ResidentialPanelIcon[j].atlas = MyAtlas.FavCimsAtlas;
							ResidentialPanelIcon[j].relativePosition = new Vector3(5f, 4f);
							ResidentialPanelText[j] = ResidentialPanelSubRow[j].AddUIComponent<UIButton>();
							ResidentialPanelText[j].name = "LabelPanelText_" + array[i] + "_" + j.ToString();
							ResidentialPanelText[j].width = 200f;
							ResidentialPanelText[j].height = 25f;
							ResidentialPanelText[j].textVerticalAlignment = UIVerticalAlignment.Middle;
							ResidentialPanelText[j].textHorizontalAlignment = 0;
							ResidentialPanelText[j].playAudioEvents = true;
							ResidentialPanelText[j].font = UIDynamicFont.FindByName("OpenSans-Regular");
							ResidentialPanelText[j].font.size = 15;
							ResidentialPanelText[j].textScale = 0.8f;
							ResidentialPanelText[j].useDropShadow = true;
							ResidentialPanelText[j].dropShadowOffset = new Vector2(1f, -1f);
							ResidentialPanelText[j].dropShadowColor = new Color32(0, 0, 0, 0);
							ResidentialPanelText[j].textPadding.left = 5;
							ResidentialPanelText[j].textPadding.right = 5;
							ResidentialPanelText[j].textColor = new Color32(51, 51, 51, 160);
							ResidentialPanelText[j].isInteractive = false;
							ResidentialPanelText[j].relativePosition = new Vector3(ResidentialPanelIcon[j].relativePosition.x + ResidentialPanelIcon[j].width, 1f);
							for (int k = 0; k < 5; k++)
							{
								ResidentialBodyRow[row] = BodyRows.AddUIComponent(typeof(ResidentialBuildingPanelRow)) as ResidentialBuildingPanelRow;
								ResidentialBodyRow[row].name = "Row_" + array[i] + "_" + k.ToString();
								ResidentialBodyRow[row].OnBuilding = 0;
								ResidentialBodyRow[row].citizen = 0U;
								ResidentialBodyRow[row].Hide();
                                row++;
							}
						}
					}
					else
					{
						if (i == 1)
						{
							WorkersPanel = BodyRows.AddUIComponent<UIPanel>();
							WorkersPanel.width = 226f;
							WorkersPanel.height = 25f;
							WorkersPanel.name = "LabelPanel_" + array[i] + "_0";
							WorkersPanel.autoLayoutDirection = LayoutDirection.Vertical;
							WorkersPanel.autoLayout = true;
							WorkersPanel.Hide();
							WorkersPanelSubRow = WorkersPanel.AddUIComponent<UIPanel>();
							WorkersPanelSubRow.width = 226f;
							WorkersPanelSubRow.height = 25f;
							WorkersPanelSubRow.name = "TitlePanel_" + array[i] + "_0";
							WorkersPanelSubRow.atlas = MyAtlas.FavCimsAtlas;
							WorkersPanelSubRow.backgroundSprite = "bg_row2";
							WorkersPanelIcon = WorkersPanelSubRow.AddUIComponent<UIButton>();
							WorkersPanelIcon.name = "LabelPanelIcon_" + array[i] + "_0";
							WorkersPanelIcon.width = 17f;
							WorkersPanelIcon.height = 17f;
							WorkersPanelIcon.atlas = MyAtlas.FavCimsAtlas;
							WorkersPanelIcon.relativePosition = new Vector3(5f, 4f);
							WorkersPanelText = WorkersPanelSubRow.AddUIComponent<UIButton>();
							WorkersPanelText.name = "LabelPanelText_" + array[i] + "_0";
							WorkersPanelText.width = 200f;
							WorkersPanelText.height = 25f;
							WorkersPanelText.textVerticalAlignment = UIVerticalAlignment.Middle;
							WorkersPanelText.textHorizontalAlignment = 0;
							WorkersPanelText.playAudioEvents = true;
							WorkersPanelText.font = UIDynamicFont.FindByName("OpenSans-Regular");
							WorkersPanelText.font.size = 15;
							WorkersPanelText.textScale = 0.8f;
							WorkersPanelText.useDropShadow = true;
							WorkersPanelText.dropShadowOffset = new Vector2(1f, -1f);
							WorkersPanelText.dropShadowColor = new Color32(0, 0, 0, 0);
							WorkersPanelText.textPadding.left = 5;
							WorkersPanelText.textPadding.right = 5;
							WorkersPanelText.textColor = new Color32(51, 51, 51, 160);
							WorkersPanelText.isInteractive = false;
							WorkersPanelText.relativePosition = new Vector3(WorkersPanelIcon.relativePosition.x + WorkersPanelIcon.width, 1f);
							int row = 0;
							for (int l = 0; l < MaxWorkersUnit*5; l++)
							{
								WorkersBodyRow[row] = BodyRows.AddUIComponent(typeof(WorkersBuildingPanelRow)) as WorkersBuildingPanelRow;
								WorkersBodyRow[row].name = "Row_" + array[i] + "_" + l.ToString();
								WorkersBodyRow[row].OnBuilding = 0;
								WorkersBodyRow[row].citizen = 0U;
								WorkersBodyRow[row].Hide();
                                row++;
							}
						}
						else
						{
							GuestsPanel = BodyRows.AddUIComponent<UIPanel>();
							GuestsPanel.width = 226f;
							GuestsPanel.height = 25f;
							GuestsPanel.name = "LabelPanel_" + array[i] + "_0";
							GuestsPanel.autoLayoutDirection = LayoutDirection.Vertical;
							GuestsPanel.autoLayout = true;
							GuestsPanel.Hide();
							GuestsPanelSubRow = GuestsPanel.AddUIComponent<UIPanel>();
							GuestsPanelSubRow.width = 226f;
							GuestsPanelSubRow.height = 25f;
							GuestsPanelSubRow.name = "TitlePanel_" + array[i] + "_0";
							GuestsPanelSubRow.atlas = MyAtlas.FavCimsAtlas;
							GuestsPanelSubRow.backgroundSprite = "bg_row2";
							GuestsPanelIcon = GuestsPanelSubRow.AddUIComponent<UIButton>();
							GuestsPanelIcon.name = "LabelPanelIcon_" + array[i] + "_0";
							GuestsPanelIcon.width = 17f;
							GuestsPanelIcon.height = 17f;
							GuestsPanelIcon.atlas = MyAtlas.FavCimsAtlas;
							GuestsPanelIcon.relativePosition = new Vector3(5f, 4f);
							GuestsPanelText = GuestsPanelSubRow.AddUIComponent<UIButton>();
							GuestsPanelText.name = "LabelPanelText_" + array[i] + "_0";
							GuestsPanelText.width = 200f;
							GuestsPanelText.height = 25f;
							GuestsPanelText.textVerticalAlignment = UIVerticalAlignment.Middle;
							GuestsPanelText.textHorizontalAlignment = 0;
							GuestsPanelText.playAudioEvents = true;
							GuestsPanelText.font = UIDynamicFont.FindByName("OpenSans-Regular");
							GuestsPanelText.font.size = 15;
							GuestsPanelText.textScale = 0.8f;
							GuestsPanelText.useDropShadow = true;
							GuestsPanelText.dropShadowOffset = new Vector2(1f, -1f);
							GuestsPanelText.dropShadowColor = new Color32(0, 0, 0, 0);
							GuestsPanelText.textPadding.left = 5;
							GuestsPanelText.textPadding.right = 5;
							GuestsPanelText.textColor = new Color32(51, 51, 51, 160);
							GuestsPanelText.isInteractive = false;
							GuestsPanelText.relativePosition = new Vector3(GuestsPanelIcon.relativePosition.x + GuestsPanelIcon.width, 1f);
							int row = 0;
							for (int m = 0; m < MaxGuestsUnit*5; m++)
							{
								GuestsBodyRow[row] = BodyRows.AddUIComponent(typeof(GuestsBuildingPanelRow)) as GuestsBuildingPanelRow;
								GuestsBodyRow[row].name = "Row_" + array[i] + "_" + m.ToString();
								GuestsBodyRow[row].OnBuilding = 0;
								GuestsBodyRow[row].citizen = 0U;
								GuestsBodyRow[row].Hide();
                                row++;
							}
						}
					}
				}
				BodyPanelScrollBar = BodySpriteBg.AddUIComponent<UIScrollablePanel>();
				BodyPanelScrollBar.name = "BodyPanelScrollBar";
				BodyPanelScrollBar.width = 10f;
				BodyPanelScrollBar.relativePosition = new Vector3(BodyRows.width + 12f, 0f);
				BodyScrollBar = BodyPanelScrollBar.AddUIComponent<UIScrollbar>();
				BodyScrollBar.width = 10f;
				BodyScrollBar.name = "BodyScrollBar";
				BodyScrollBar.orientation = UIOrientation.Vertical;
				BodyScrollBar.pivot = UIPivotPoint.TopRight;
				BodyScrollBar.AlignTo(BodyScrollBar.parent, 0);
				BodyScrollBar.minValue = 0f;
				BodyScrollBar.value = 0f;
				BodyScrollBar.incrementAmount = 25f;
				BodyPanelTrackSprite = BodyScrollBar.AddUIComponent<UISlicedSprite>();
				BodyPanelTrackSprite.autoSize = true;
				BodyPanelTrackSprite.name = "BodyScrollBarTrackSprite";
				BodyPanelTrackSprite.fillDirection = UIFillDirection.Vertical;
				BodyPanelTrackSprite.atlas = MyAtlas.FavCimsAtlas;
				BodyPanelTrackSprite.spriteName = "scrollbartrack";
				BodyPanelTrackSprite.relativePosition = BodyScrollBar.relativePosition;
				BodyScrollBar.trackObject = BodyPanelTrackSprite;
				thumbSprite = BodyScrollBar.AddUIComponent<UISlicedSprite>();
				thumbSprite.name = "BodyScrollBarThumbSprite";
				thumbSprite.autoSize = true;
				thumbSprite.width = thumbSprite.parent.width;
				thumbSprite.fillDirection = UIFillDirection.Vertical;
				thumbSprite.atlas = MyAtlas.FavCimsAtlas;
				thumbSprite.spriteName = "scrollbarthumb";
				thumbSprite.relativePosition = BodyScrollBar.relativePosition;
				BodyScrollBar.thumbObject = thumbSprite;
				BodyRows.verticalScrollbar = BodyScrollBar;
				BodyRows.eventMouseWheel += delegate(UIComponent component, UIMouseEventParameter eventParam)
				{
					int sign = Math.Sign(eventParam.wheelDelta);
					BodyRows.scrollPosition += new Vector2(0f, (sign * -1) * BodyScrollBar.incrementAmount);
				};
				Footer = AddUIComponent<UIPanel>();
				Footer.name = "PeopleInsideBuildingsPanelFooter";
				Footer.width = width;
				Footer.height = 12f;
				Footer.relativePosition = new Vector3(0f, Title.height + Body.height);
				FooterSpriteBg = Footer.AddUIComponent<UITextureSprite>();
				FooterSpriteBg.width = Footer.width;
				FooterSpriteBg.height = Footer.height;
				FooterSpriteBg.texture = TextureDB.VehiclePanelFooterBackground;
				FooterSpriteBg.relativePosition = Vector3.zero;
				UIComponent uicomponent = UIView.Find<UIButton>("Esc");
				if (uicomponent != null)
				{
					uicomponent.eventClick += delegate(UIComponent component, UIMouseEventParameter eventParam)
					{
						Hide();
					};
				}
			}
			catch (Exception ex)
			{
                Utils.Debug.Error(" Building Panel Start() : " + ex.ToString());
			}
		}

		public override void Update()
		{
			bool unLoading = FavCimsMainClass.UnLoading;
			if (!unLoading)
			{
				bool isEmpty = BuildingID.IsEmpty;
				if (isEmpty)
				{
					bool garbage = Garbage;
					if (garbage)
					{
						Wait = true;
						CimsOnBuilding.Clear();
						WorkersCount = 0;
						GuestsCount = 0;
						try
						{
							if (buildingInfo.m_class.m_service == ItemClass.Service.Residential)
							{
								int row = 0;
								for (int i = 0; i < 26; i++)
								{
									ResidentialPanels[i].Hide();
									for (int j = 0; j < 5; j++)
									{
										ResidentialBodyRow[row].Hide();
										ResidentialBodyRow[row].citizen = 0U;
										ResidentialBodyRow[row].OnBuilding = 0;
										ResidentialBodyRow[row].firstRun = true;
                                        row++;
									}
								}
							}
							else
							{
								if (buildingInfo.m_class.m_service == ItemClass.Service.Commercial)
								{
									WorkersPanel.Hide();
									for (int k = 0; k < MaxWorkersUnit*5; k++)
									{
										WorkersBodyRow[k].Hide();
										WorkersBodyRow[k].citizen = 0U;
										WorkersBodyRow[k].OnBuilding = 0;
										WorkersBodyRow[k].firstRun = true;
									}
									GuestsPanel.Hide();
									for (int l = 0; l < MaxGuestsUnit*5; l++)
									{
										GuestsBodyRow[l].Hide();
										GuestsBodyRow[l].citizen = 0U;
										GuestsBodyRow[l].OnBuilding = 0;
										GuestsBodyRow[l].firstRun = true;
									}
								}
								else
								{
									WorkersPanel.Hide();
									for (int m = 0; m < MaxWorkersUnit * 5; m++)
									{
										WorkersBodyRow[m].Hide();
										WorkersBodyRow[m].citizen = 0U;
										WorkersBodyRow[m].OnBuilding = 0;
										WorkersBodyRow[m].firstRun = true;
									}
								}
							}
							Wait = false;
						}
						catch
						{
						}
						Garbage = false;
					}
					firstRun = true;
				}
				else
				{
					try
					{
						buildingInfo = MyBuilding.m_buildings.m_buffer[BuildingID.Building].Info;
						if (!WorldInfoPanel.GetCurrentInstanceID().IsEmpty && WorldInfoPanel.GetCurrentInstanceID() != BuildingID)
						{
							Wait = true;
							CimsOnBuilding.Clear();
							WorkersCount = 0;
							GuestsCount = 0;
							if (buildingInfo.m_class.m_service == ItemClass.Service.Residential)
							{
								int i = 0;
								for (int n = 0; n < MaxHouseHolds; n++)
								{
									ResidentialPanels[n].Hide();
									for (int b = 0; b < 5; b++)
									{
										ResidentialBodyRow[i].Hide();
										ResidentialBodyRow[i].citizen = 0U;
										ResidentialBodyRow[i].OnBuilding = 0;
										ResidentialBodyRow[i].firstRun = true;
                                        i++;
									}
								}
							}
							else
							{
								if (buildingInfo.m_class.m_service == ItemClass.Service.Commercial)
								{
									WorkersPanel.Hide();
									for (int a = 0; a < MaxWorkersUnit*5; a++)
									{
										WorkersBodyRow[a].Hide();
										WorkersBodyRow[a].citizen = 0U;
										WorkersBodyRow[a].OnBuilding = 0;
										WorkersBodyRow[a].firstRun = true;
									}
									GuestsPanel.Hide();
									for (int a = 0; a < MaxGuestsUnit*5; a++)
									{
										GuestsBodyRow[a].Hide();
										GuestsBodyRow[a].citizen = 0U;
										GuestsBodyRow[a].OnBuilding = 0;
										GuestsBodyRow[a].firstRun = true;
									}
								}
								else
								{
									WorkersPanel.Hide();
									for (int a = 0; a < MaxWorkersUnit*5; a++)
									{
										WorkersBodyRow[a].Hide();
										WorkersBodyRow[a].citizen = 0U;
										WorkersBodyRow[a].OnBuilding = 0;
										WorkersBodyRow[a].firstRun = true;
									}
								}
							}
							BuildingID = WorldInfoPanel.GetCurrentInstanceID();
							bool isEmpty2 = BuildingID.IsEmpty;
							if (isEmpty2)
							{
								return;
							}
							Wait = false;
						}
						if (isVisible && !BuildingID.IsEmpty)
						{
							Garbage = true;
							absolutePosition = new Vector3(RefPanel.absolutePosition.x + RefPanel.width + 5f, RefPanel.absolutePosition.y);
							height = RefPanel.height - 15f;
							if (50f + CimsOnBuilding.Count * 25f < height - Title.height - Footer.height)
							{
								Body.height = height - Title.height - Footer.height;
							}
							else
							{
								if (50f + CimsOnBuilding.Count * 25f > 400f)
								{
									Body.height = 400f;
								}
								else
								{
									Body.height = 50f + CimsOnBuilding.Count * 25f;
								}
							}
							BodySpriteBg.height = Body.height;
							Footer.relativePosition = new Vector3(0f, Title.height + Body.height);
							BodyRows.height = Body.height;
							BodyPanelScrollBar.height = Body.height;
							BodyScrollBar.height = Body.height;
							BodyPanelTrackSprite.size = BodyPanelTrackSprite.parent.size;
							seconds -= 1f * Time.deltaTime;
							if (seconds <= 0f || firstRun)
							{
								execute = true;
								seconds = 0.5f;
							}
							else
							{
								execute = false;
							}
							if (execute)
							{
								firstRun = false;
								BuildingUnits = MyBuilding.m_buildings.m_buffer[(int)BuildingID.Building].m_citizenUnits;

								int unitnum = 0;
								int rownum = 0;

								int limit;
								if (buildingInfo.m_class.m_service == ItemClass.Service.Residential)
								{
                                    limit = MaxHouseHolds;
								}
								else
								{
									if (buildingInfo.m_class.m_service == ItemClass.Service.Commercial)
									{
                                        limit = MaxGuestsUnit;
									}
									else
									{
                                        limit = MaxWorkersUnit;
									}
								}
								while (BuildingUnits != 0U && unitnum < limit)
								{
									uint nextUnit = MyCitizen.m_units.m_buffer[(int)BuildingUnits].m_nextUnit;
									for (int i = 0; i < 5; i++)
									{
										uint citizen = MyCitizen.m_units.m_buffer[(int)BuildingUnits].GetCitizen(i);
										Citizen citizen2 = MyCitizen.m_citizens.m_buffer[(int)citizen];
										if (citizen != 0U && !CimsOnBuilding.ContainsKey(citizen))
										{
											if (buildingInfo.m_class.m_service == ItemClass.Service.Residential)
											{
												TitleBuildingName.text = FavCimsLang.Text("Citizens_HouseHoldsTitle");
												if (ResidentialPanels[unitnum] != null)
												{
													ResidentialPanels[unitnum].Show();
													ResidentialPanelIcon[unitnum].normalFgSprite = "BapartmentIcon";
													ResidentialPanelText[unitnum].text = FavCimsLang.Text("OnBuilding_Residential") + " " + (unitnum + 1).ToString();
													if (ResidentialBodyRow[rownum].citizen != 0U && CimsOnBuilding.ContainsKey(ResidentialBodyRow[rownum].citizen))
													{
														Wait = true;
														CimsOnBuilding.Remove(ResidentialBodyRow[rownum].citizen);
													}
													CimsOnBuilding.Add(citizen, BuildingUnits);
													ResidentialBodyRow[rownum].OnBuilding = BuildingID.Building;
													ResidentialBodyRow[rownum].citizen = citizen;
													ResidentialBodyRow[rownum].LocType = 0;
													ResidentialBodyRow[rownum].firstRun = true;
													ResidentialBodyRow[rownum].Show();
													bool wait = Wait;
													if (wait)
													{
														Wait = false;
													}
												}
											}
											else
											{
												if (buildingInfo.m_class.m_service == ItemClass.Service.Industrial || buildingInfo.m_class.m_service == ItemClass.Service.Office)
												{
													TitleBuildingName.text = FavCimsLang.Text("WorkersOnBuilding");
													WorkersPanel.Show();
													WorkersPanelIcon.normalFgSprite = "BworkingIcon";
													WorkersPanelText.text = FavCimsLang.Text("OnBuilding_Workers");
													if (citizen2.GetBuildingByLocation() == BuildingID.Building && citizen2.CurrentLocation != Citizen.Location.Moving)
													{
														WorkersCount++;
			
														if (WorkersPanel != null && WorkersBodyRow[unitnum] != null)
														{
															if (WorkersBodyRow[rownum].citizen != 0U && CimsOnBuilding.ContainsKey(WorkersBodyRow[rownum].citizen))
															{
																Wait = true;
																CimsOnBuilding.Remove(WorkersBodyRow[rownum].citizen);
															}
															CimsOnBuilding.Add(citizen, BuildingUnits);
															WorkersBodyRow[rownum].OnBuilding = BuildingID.Building;
															WorkersBodyRow[rownum].citizen = citizen;
															WorkersBodyRow[rownum].LocType = Citizen.Location.Work;
															WorkersBodyRow[rownum].firstRun = true;
															WorkersBodyRow[rownum].Show();
															bool wait2 = Wait;
															if (wait2)
															{
																Wait = false;
															}
														}
													}
													if (WorkersCount == 0)
													{
														WorkersPanelText.text = FavCimsLang.Text("OnBuilding_NoWorkers");
													}
												}
												else
												{
													TitleBuildingName.text = FavCimsLang.Text("CitizenOnBuildingTitle");
													if (BuildingID.Building == citizen2.m_workBuilding)
													{
														WorkersPanel.Show();
														WorkersPanelIcon.normalFgSprite = "BworkingIcon";
														WorkersPanelText.text = FavCimsLang.Text("OnBuilding_Workers");
														if (citizen2.GetBuildingByLocation() == BuildingID.Building && citizen2.CurrentLocation != Citizen.Location.Moving)
														{
															WorkersCount++;
															if (WorkersPanel != null && WorkersBodyRow[unitnum] != null)
															{
																if (WorkersBodyRow[rownum].citizen != 0U && CimsOnBuilding.ContainsKey(WorkersBodyRow[rownum].citizen))
																{
																	Wait = true;
																	CimsOnBuilding.Remove(WorkersBodyRow[rownum].citizen);
																}
																CimsOnBuilding.Add(citizen, BuildingUnits);
																WorkersBodyRow[rownum].OnBuilding = BuildingID.Building;
																WorkersBodyRow[rownum].citizen = citizen;
																WorkersBodyRow[rownum].LocType = Citizen.Location.Work;
																WorkersBodyRow[rownum].firstRun = true;
																WorkersBodyRow[rownum].Show();
																bool wait3 = Wait;
																if (wait3)
																{
																	Wait = false;
																}
															}
														}
														if (WorkersCount == 0)
														{
															WorkersPanelText.text = FavCimsLang.Text("OnBuilding_NoWorkers");
														}
													}
													else
													{
														GuestsPanel.Show();
														GuestsPanelIcon.normalFgSprite = "BcommercialIcon";
														GuestsPanelText.text = FavCimsLang.Text("OnBuilding_Guests");
														if (citizen2.GetBuildingByLocation() == BuildingID.Building && citizen2.CurrentLocation != Citizen.Location.Moving)
														{
															GuestsCount++;
															if (GuestsPanel != null && GuestsBodyRow[unitnum] != null)
															{
																if (GuestsBodyRow[rownum].citizen != 0U && CimsOnBuilding.ContainsKey(GuestsBodyRow[rownum].citizen))
																{
																	Wait = true;
																	CimsOnBuilding.Remove(GuestsBodyRow[rownum].citizen);
																}
																CimsOnBuilding.Add(citizen, BuildingUnits);
																GuestsBodyRow[rownum].OnBuilding = BuildingID.Building;
																GuestsBodyRow[rownum].citizen = citizen;
																GuestsBodyRow[rownum].LocType = Citizen.Location.Visit;
																GuestsBodyRow[rownum].firstRun = true;
																GuestsBodyRow[rownum].Show();
																bool wait4 = Wait;
																if (wait4)
																{
																	Wait = false;
																}
															}
														}
														if (GuestsCount == 0)
														{
															GuestsPanelText.text = FavCimsLang.Text("OnBuilding_NoGuests");
														}
													}
												}
											}
										}
                                        rownum++;
									}
									if (BuildingUnits == 0U && buildingInfo.m_class.m_service == ItemClass.Service.Residential)
									{
										ResidentialPanels[unitnum].Hide();
									}
									BuildingUnits = nextUnit;
									if (++unitnum > 524288)
									{
										break;
									}
								}
							}
						}
					}
					catch
					{
					}
				}
			}
		}
	}
}
