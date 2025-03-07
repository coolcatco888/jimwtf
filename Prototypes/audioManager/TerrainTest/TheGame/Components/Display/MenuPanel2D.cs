﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheGame.Components.Display
{
    /// <summary>
    /// A richer panel specifically for menus. This menu keeps track of
    /// which menu item is currently selected.
    /// </summary>
    class MenuPanel2D : PanelComponent2D
    {
        private Color highlightColor;

        public Color HighlightColor
        {
            get { return highlightColor; }
        }

        private int startIndex, endIndex, currentIndex;

        public int StartIndex
        {
            get { return startIndex; }
        }

        public int EndIndex
        {
            get { return endIndex; }
        }

        public int CurrentIndex
        {
            get { return currentIndex; }
        }

        private MenuPanel2D(GameScreen parent, Vector2 position)
            : base(parent, position)
        {
        }
        /// <summary>
        /// Creates a menu from an existing panel. The panel you pass in must have the menu 
        /// options inserted consecutively otherwise selection and highlighting will not work. Default font selected color is Yellow.
        /// 
        /// Note that this panel is zero indexed and the first component inserted should be
        /// an ImageComponent2D background image. This image will be at index zero
        /// </summary>
        /// <param name="panel">Menu components</param>
        /// <param name="startIndex">index of first text component in menu</param>
        /// <param name="endIndex">index of last text component in menu</param>
        /// <returns></returns>
        public static MenuPanel2D CreateMenuPanel2D(PanelComponent2D panel, int startIndex, int endIndex)
        {
            return CreateMenuPanel2D(panel, Color.Yellow, startIndex, endIndex);
        }

        /// <summary>
        /// Creates a menu from an existing panel. The panel you pass in must have the menu
        /// options inserted consecutively otherwise selection and highlighting will not work.
        /// 
        /// Note that this panel is zero indexed and the first component inserted should be
        /// an ImageComponent2D background image. This image will be at index zero
        /// </summary>
        /// <param name="panel">Menu components</param>
        /// <param name="highlightColor">Highlighted color of selected item</param>
        /// <param name="startIndex">index of first text component in menu</param>
        /// <param name="endIndex">index of last text component in menu</param>
        /// <returns></returns>
        public static MenuPanel2D CreateMenuPanel2D(PanelComponent2D panel, Color highlightColor,  int startIndex, int endIndex)
        {
            MenuPanel2D newPanel = new MenuPanel2D(panel.Parent, panel.Position);
            newPanel.highlightColor = highlightColor;
            newPanel.startIndex = startIndex;
            newPanel.currentIndex = startIndex;
            newPanel.endIndex = endIndex;
            newPanel.PanelItems = ConvertTextToMenuEntries(panel.PanelItems, newPanel);
            return newPanel;
        }

        private static PanelComponents ConvertTextToMenuEntries(PanelComponents components, MenuPanel2D owner)
        {
            PanelComponents newComponents = new PanelComponents(owner);

            int i = 0;
            foreach (DisplayComponent2D component in components)
            {
                DisplayComponent2D current = component;
                if (component is TextComponent2D)
                {
                    current = MenuTextComponent2D.CreateMenuTextComponent2D((TextComponent2D) component, owner.HighlightColor, i == owner.StartIndex);
                    owner.Parent.Components.Remove(component);
                }
                current.Position = current.Position - owner.Position;
                newComponents.Add(current);
                i++;
            }

            return newComponents;
        }

        /// <summary>
        /// Highlight next item in menu list
        /// </summary>
        public void Next()
        {
            ((MenuTextComponent2D)panelItems[currentIndex]).Selected = false;
            currentIndex = currentIndex == endIndex ? startIndex : currentIndex + 1;
            ((MenuTextComponent2D)panelItems[currentIndex]).Selected = true;
        }

        /// <summary>
        /// Highlight previous item in menu list 
        /// </summary>
        public void Previous()
        {
            ((MenuTextComponent2D)panelItems[currentIndex]).Selected = false;
            currentIndex = currentIndex == startIndex ? endIndex : currentIndex - 1;
            ((MenuTextComponent2D)panelItems[currentIndex]).Selected = true;
        }

        /// <summary>
        /// Gets the text of the currently selected menu item.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentText()
        {
            string text = null;

            if (currentIndex >= 0 && currentIndex < panelItems.Count)
            {
                I2DComponent item = panelItems.ElementAt(currentIndex);
                if (item is TextComponent2D)
                {
                    text = ((TextComponent2D)item).Text;
                }
            }
            return text;
        }

        /// <summary>
        /// Gets a menu item if the cursor is hovering over it
        /// </summary>
        /// <param name="cursorPosition"></param>
        /// <returns></returns>
        public string GetCursorSelectedText(Point cursorPosition)
        {
            string text = null;

            int i = 0;
            foreach (Component item in panelItems)
            {

                if (item is TextComponent2D)
                {
                    TextComponent2D textItem = (TextComponent2D) item;
                    Vector2 size = textItem.Font.MeasureString(textItem.Text);
                    Rectangle rect = new Rectangle((int)(textItem.Position.X + position.X), (int) (textItem.Position.Y + position.Y), (int)size.X, (int) size.Y);
                    if(rect.Contains(cursorPosition))
                    {
                        text = textItem.Text;
                        currentIndex = i;
                    }
                }
                i++;
            }
            return text;
        }

    }
}
