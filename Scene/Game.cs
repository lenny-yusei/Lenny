﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Word.Scene
{
    sealed class Game : asd.Scene
    {
        public Game()
        {
            var backgroundLayer = new asd.Layer2D();
            var background = new asd.TextureObject2D();
            background.Texture = asd.Engine.Graphics.CreateTexture2D("Resource/background.png");
            backgroundLayer.AddObject(background);
            AddLayer(backgroundLayer);

            player = new Charactor.Player(gameLayer);
            gameLayer.AddObject(player);

            enemy = new Charactor.Enemy(gameLayer, 'Z');
            gameLayer.AddObject(enemy);

            AddLayer(gameLayer);
        }

        protected override void OnUpdated()
        {
            if (!enemy.IsAlive)
                asd.Engine.ChangeScene(new Scene.Clear());
            if (!player.IsAlive)
                asd.Engine.ChangeScene(new Scene.GameOver());
            CollisionPlayerAndBullet();
            CollisionEnemyAndShot();
        }

        private void CollisionPlayerAndBullet()
        {
            Func<bool> isHit = () =>
            {
                foreach (var obj in gameLayer.Objects)
                {
                    if (!(obj is Charactor.Bullet))
                        continue;
                    if (player.IsHit(obj as Charactor.Bullet))
                        return true;
                }
                return false;
            };

            if (isHit())
            {
                player.damage();
                gameLayer.Objects.ToList().RemoveAll(o =>
                {
                    if (!(o is Charactor.Bullet))
                        return false;
                    o.Dispose();
                    return true;
                });
            }

        }

        private void CollisionEnemyAndShot()
        {
            gameLayer.Objects.ToList().RemoveAll(o =>
            {
                if (!(o is Charactor.Shot))
                    return false;
                if (!(enemy.IsHit(o as Charactor.Shot)))
                   return false; 
                
                o.Dispose();
                enemy.damage(o as Charactor.Shot);
                return true;
            }); 
        }
        private Charactor.Player player;
        private Charactor.Enemy enemy;
        private asd.Layer2D gameLayer = new asd.Layer2D();
    }
}
