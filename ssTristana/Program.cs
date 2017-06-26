using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Color = System.Drawing.Color;

namespace ss2
{
    class Program
    {
        public static Menu Menu, DrawMenu, ComboMenu;
        public static Spell.Active Q;
        public static Spell.Skillshot W;
        public static Spell.Targeted E;
        public static Spell.Targeted R;

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }


        }


        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
            Drawing.OnDraw += Game_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
        }
        private static void Game_OnStart(EventArgs args)
        {
            Chat.Print("ssTristana Yüklendi");
            Chat.Print("Bu Addon Safa Soylu için Yapılmıştır !");
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular);
            E = new Spell.Targeted(SpellSlot.E, 575);
            R = new Spell.Targeted(SpellSlot.R, 575);




            Menu = MainMenu.AddMenu("ssTrist", "Tris");
            Menu.AddSeparator();
            Menu.AddLabel("Hey Trist");
            DrawMenu = Menu.AddSubMenu("TristSub", "DrawMenu");
            DrawMenu.Add("ÇizgileriİptalEt", new CheckBox("Çizgiler İptal Olsun mu", true));
            ComboMenu = Menu.AddSubMenu("KomboMenu", "ssTristanaCombo");
            ComboMenu.Add("KomboQ", new CheckBox("KomboQ", true));
            ComboMenu.Add("KomboW", new CheckBox("KomboW", true));
            ComboMenu.Add("KomboE", new CheckBox("KomboE", true));
            ComboMenu.Add("KomboR", new CheckBox("KomboR", true));
        }
        public static void Game_OnDraw(EventArgs args)
        {
            //new Circle() { Color = Color.White, Radius = ObjectManager.Player.GetAutoAttackRange(), BorderWidth= 2f }.Draw(ObjectManager.Player.Position);
            if (DrawMenu["ÇizgileriİptalEt"].Cast<CheckBox>().CurrentValue)
            {
                new Circle() { Color = Color.White, Radius = ObjectManager.Player.GetAutoAttackRange(), BorderWidth = 2f }.Draw(ObjectManager.Player.Position);
            }

        }

        public static void Game_OnUpdate(EventArgs args)
        {
            var hedef = TargetSelector.GetTarget(1000, DamageType.Physical);
            if (hedef.IsValid()) return;
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo)
            {
                if (Q.IsReady() && ComboMenu["KomboQ"].Cast<CheckBox>().CurrentValue)
                {
                    Q.Cast();
                }
                if (W.IsReady() && _Player.Distance(hedef) <= W.Range + _Player.GetAutoAttackRange() && ComboMenu["KomboW"].Cast<CheckBox>().CurrentValue)
                {
                    W.Cast(hedef);
                }
                if (E.IsReady() && E.IsInRange(hedef) && ComboMenu["KomboE"].Cast<CheckBox>().CurrentValue)
                {
                    E.Cast(hedef);
                }
                if (R.IsReady() && R.IsInRange(hedef) && ComboMenu["KomboR"].Cast<CheckBox>().CurrentValue)
                {
                    R.Cast(hedef);

                }
            }



        }

    }
}
