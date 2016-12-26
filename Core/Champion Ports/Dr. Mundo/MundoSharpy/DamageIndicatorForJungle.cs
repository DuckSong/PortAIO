using System;
using System.Collections.Generic;
using System.Linq;

using LeagueSharp;

using Color = System.Drawing.Color;

using EloBuddy; 
 using LeagueSharp.Common; 
 namespace Mundo_Sharpy
{
    class DamageIndicatorForJungle
    {
        public delegate float DamageToUnitDelegate(Obj_AI_Minion hero);

        public static Color Color = Color.Lime;
        public static Color FillColor = Color.Goldenrod;
        public static bool Fill = true;

        public static bool Enabled = true;
        private static DamageToUnitDelegate _damageToUnit;

        public static List<JungleMobOffsets> JungleMobOffsetsList = new List<JungleMobOffsets>()
        {
            new JungleMobOffsets { BaseSkinName = "SRU_Red", Width = 139, Height = 4, XOffset = 6, YOffset= 22 },
            new JungleMobOffsets { BaseSkinName = "SRU_RedMini", Width = 49, Height = 2, XOffset = 36, YOffset= 22 },
            new JungleMobOffsets { BaseSkinName = "SRU_Blue", Width = 139, Height = 4, XOffset = 6, YOffset= 22 },
            new JungleMobOffsets { BaseSkinName = "SRU_BlueMini", Width = 49, Height = 2, XOffset = 36, YOffset= 22 },
            new JungleMobOffsets { BaseSkinName = "SRU_BlueMini2", Width = 49, Height = 2, XOffset = 36, YOffset= 22 },
            new JungleMobOffsets { BaseSkinName = "SRU_Gromp", Width = 86, Height = 2, XOffset = 62, YOffset= 22 },
            new JungleMobOffsets { BaseSkinName = "Sru_Crab", Width = 60, Height = 2, XOffset = 45, YOffset= 36 },
            new JungleMobOffsets { BaseSkinName = "SRU_Dragon", Width = 140, Height = 4, XOffset = 5, YOffset= 22 },
            new JungleMobOffsets { BaseSkinName = "SRU_Baron", Width = 190, Height = 4, XOffset = -20, YOffset= 22 },
            new JungleMobOffsets { BaseSkinName = "SRU_Krug", Width = 80, Height = 2, XOffset = 58, YOffset= 22 },
            new JungleMobOffsets { BaseSkinName = "SRU_KrugMini", Width = 55, Height = 2, XOffset = 40, YOffset= 20 },
            new JungleMobOffsets { BaseSkinName = "SRU_Razorbeak", Width = 74, Height = 2, XOffset = 53, YOffset= 22 },
            new JungleMobOffsets { BaseSkinName = "SRU_RazorbeakMini", Width = 49, Height = 2, XOffset = 36, YOffset= 20 },
            new JungleMobOffsets { BaseSkinName = "SRU_Murkwolf", Width = 74, Height = 2, XOffset = 53, YOffset= 22 },
            new JungleMobOffsets { BaseSkinName = "SRU_MurkwolfMini", Width = 55, Height = 2, XOffset = 40, YOffset= 20 },
            //new JungleMobOffsets { BaseSkinName = "SRU_ChaosMinionMelee", Width = 62, Height = 2, XOffset = 44, YOffset= 21 },
            //new JungleMobOffsets { BaseSkinName = "SRU_ChaosMinionSiege", Width = 60, Height = 2, XOffset = 44, YOffset= 21 },
            //new JungleMobOffsets { BaseSkinName = "SRU_ChaosMinionSuper", Width = 55, Height = 2, XOffset = 44, YOffset= 21 },
            //new JungleMobOffsets { BaseSkinName = "SRU_ChaosMinionRanged", Width = 62, Height = 2, XOffset = 44, YOffset= 21 },
            //new JungleMobOffsets { BaseSkinName = "SRU_OrderMinionMelee", Width = 62, Height = 2, XOffset = 44, YOffset= 21 },
            //new JungleMobOffsets { BaseSkinName = "SRU_OrderMinionSiege", Width = 60, Height = 2, XOffset = 44, YOffset= 21 },
            //new JungleMobOffsets { BaseSkinName = "SRU_OrderMinionSuper", Width = 55, Height = 2, XOffset = 44, YOffset= 21 },
            //new JungleMobOffsets { BaseSkinName = "SRU_OrderMinionRanged", Width = 62, Height = 2, XOffset = 44, YOffset= 21 }
        };

        public static DamageToUnitDelegate DamageToUnit
        {
            get { return _damageToUnit; }

            set
            {
                if (_damageToUnit == null)
                {
                    Drawing.OnDraw += Drawing_OnDraw;
                }
                _damageToUnit = value;
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (!Enabled || _damageToUnit == null)
            {
                return;
            }

            foreach (var unit in ObjectManager.Get<Obj_AI_Minion>().Where(h => h.IsHPBarRendered && h.IsValid && h.Team == GameObjectTeam.Neutral))
            {
                if (_damageToUnit(unit) > 2)
                {
                    var MobOffset = JungleMobOffsetsList.Find(x => x.BaseSkinName == unit.CharData.BaseSkinName);
                    if (MobOffset != null)
                    {
                        var barPos = unit.HPBarPosition;
                        barPos.X += MobOffset.XOffset;
                        barPos.Y += MobOffset.YOffset;

                        var damage = _damageToUnit(unit);

                        if (damage > 0)
                        {
                            var HPPercent = (unit.Health / unit.MaxHealth) * 100;
                            var HPPrecentAfterDamage = ((unit.Health - damage) / unit.MaxHealth) * 100;
                            float DrawStartXPos = barPos.X + (MobOffset.Width * (HPPrecentAfterDamage / 100));
                            float DrawEndXPos = barPos.X + (MobOffset.Width * (HPPercent / 100));

                            if (unit.Health < damage)
                            {
                                DrawStartXPos = barPos.X;
                            }

                            Drawing.DrawLine(DrawStartXPos, barPos.Y, DrawEndXPos, barPos.Y, MobOffset.Height, FillColor);
                        }
                    }
                }
            }
        }
    }

    class JungleMobOffsets
    {
        public string BaseSkinName;
        public int XOffset;
        public int YOffset;
        public int Width;
        public int Height;
    }
}
