using System;
using System.Collections.Generic;
using System.Drawing;

namespace Lobbies.LobbyBehaviour
{
    public class SlotColor
    {
        public Guid Id { get; } = Guid.NewGuid();
        public KnownColor Color { get; set; }
        public List<KnownColor> Colors { get; set; }
        public List<KnownColor> AvailableColors { get; set; }

        public SlotColor(List<KnownColor> colors, List<KnownColor> availableColors, KnownColor playerColor)
        {
            Colors = colors;
            AvailableColors = availableColors;
            Color = playerColor;
        }

        public bool ChangeColor(KnownColor color)
        {
            if (!AvailableColors.Contains(color))
                return false;

            AvailableColors.Add(Color);
            AvailableColors.Remove(color);
            Color = color;

            return true;
        }
    }
}