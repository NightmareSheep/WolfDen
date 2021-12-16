using Lupus.Behaviours.Defend;
using Lupus.Other;
using Lupus.Tiles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Units
{
    public class Unit : IUnit, IDestroy
    {
        public event EventHandler<Tile> TileIsSetEvent;
        protected List<IDestroy> Destroyables = new List<IDestroy>();

        public Unit(Game game, Player owner, string id, Tile tile)
        {
            Game = game;
            Owner = owner;
            Id = id;
            Tile = tile;
        }

        private Game Game { get; }
        private Tile tile;
        private Player owner;

        public Health Health { get; protected set; }
        public string Id { get; }

        public Tile Tile
        {
            get { return tile; }
            set
            {
                if (tile != value)
                {
                    if (tile != null)
                        tile.Unit = null;


                    tile = value;

                    if (value != null)
                        value.Unit = this;

                    TileIsSetEvent?.Invoke(this, tile);
                }              
                
            }
        }
        
        public Player Owner
        {
            get { return owner; }
            set
            {
                owner = value ?? throw new Exception("Owner of unit cannot be null");
            }
        }

        public async virtual Task Destroy()
        {
            Tile = null;
            Game.GameObjects.Remove(Id);
            Owner.GameObjects.Remove(Id);

            foreach(var destroyable in Destroyables)
            {
                await destroyable.Destroy();
            }
        }
    }
}
