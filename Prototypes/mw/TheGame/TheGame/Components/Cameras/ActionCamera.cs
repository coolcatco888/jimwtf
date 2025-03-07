﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TheGame.Components.Cameras
{
    class ActionCamera : Camera
    {
        private float minDistance, maxDistance, minHeight, maxHeight;

        private ActorList actorsToFollow;

        private const float distancePerUpdate = 0.0025f, zoomConstant = 0.4f;

        private float initAngle = 0.0f;

        /// <summary>
        /// Closest Distance the camera can be to the actors
        /// </summary>
        public float MinDistance
        {
            get { return minDistance; }
            set { minDistance = value; }
        }

        /// <summary>
        /// Lowest the camera can be
        /// </summary>
        public float MinHeight
        {
            get { return minHeight; }
            set { minHeight = value; }
        }

        /// <summary>
        /// Highest the camera can be
        /// </summary>
        public float MaxHeight
        {
            get { return maxHeight; }
            set { maxHeight = value; }
        }


        /// <summary>
        /// A group of actors the camera needs to capture
        /// </summary>
        public ActorList ActorsToFollow
        {
            get { return actorsToFollow; }
            set { actorsToFollow = value; }
        }

        /// <summary>
        /// Creates an action camera with standard settings
        /// </summary>
        /// <param name="parent">Screen the camera is contained in</param>
        public ActionCamera(GameScreen parent)
            : this(parent, 4.5f, 12.0f, 10.0f, 12.0f, new ActorList())
        {
        }

        /// <summary>
        /// Creates an action camera with variable minimum follow distance, minimum height, maximum height, and actors to follow
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="minDistance"></param>
        /// <param name="minHeight"></param>
        /// <param name="maxHeight"></param>
        /// <param name="actorsToFollow"></param>
        public ActionCamera(GameScreen parent, float minDistance, float maxDistance, float minHeight, float maxHeight, ActorList actorsToFollow)
            : base(parent)
        {
            this.minDistance = minDistance;
            this.maxDistance = maxDistance;
            this.minHeight = minHeight;
            this.maxHeight = maxHeight;
            this.actorsToFollow = actorsToFollow;
           
        }

        public override void Update(GameTime gameTime)
        {

            float sumX = 0, sumY = 0, sumZ = 0, count = 0;

            float distOfFurthestActorFromLookAt = minDistance;
            if (actorsToFollow != null)
            {
                //Sum all the positions
                foreach (Billboard actor in actorsToFollow)
                {
                    if (actor.IsDisposed)
                        continue;

                    sumX += actor.Position.X;
                    sumY += actor.Position.Y;
                    sumZ += actor.Position.Z;

                    count++;
                }

                //Change the lookAtPosition to be the average position of all of the actors
                if (count > 0)
                {
                    Vector3 newLookAt = new Vector3(sumX / count, sumY / count, sumZ / count);
                    newLookAt -= lookAt;
                    float length = newLookAt.Length();
                    float velocity = length * distancePerUpdate;
                    newLookAt.Normalize();
                    newLookAt *= velocity * gameTime.ElapsedGameTime.Milliseconds;
                    lookAt += newLookAt;
                }
                distOfFurthestActorFromLookAt = CalculateCameraZoomDistance(distOfFurthestActorFromLookAt);
                ChangeCameraPosition(distOfFurthestActorFromLookAt, gameTime);

                base.Update(gameTime);

            }


        }

        private float CalculateCameraZoomDistance(float distOfFurthestActorFromLookAt)
        {

            foreach (Billboard actor in actorsToFollow)
            {
                if (actor.IsDisposed)
                    continue;

                float distanceFromCamera = (actor.Position - position).Length();
                float distanceFromLookAt = (actor.Position - lookAt).Length();

                //Here we determine percentage of the distance from look at * the zoomConstant we want to add back
                float denominator = distanceFromCamera > distanceFromLookAt ? distanceFromCamera : distanceFromLookAt;
                float additiveRatio = Math.Abs(distanceFromCamera - distanceFromLookAt) / denominator;
                additiveRatio += 0.5f;

                //Here we determine percentage of the distance from look at * the zoomConstant we want to subtract
                Vector3 lineOfSight = position - lookAt;
                if (lineOfSight != Vector3.Zero)
                {
                    lineOfSight.Normalize();
                    Vector3 line = new Vector3(position.X, 0.0f, position.Z) - lookAt;
                    line.Normalize();
                    float cameraUpAndDownAngle = (float)Math.Acos(Vector3.Dot(lineOfSight, line));

                    if (initAngle == 0.0f)
                        initAngle = cameraUpAndDownAngle;

                    float subtractionRatio = Math.Abs(initAngle - cameraUpAndDownAngle);

                    //Calculate the current distance and the distance we want to subtract
                    float currentDist = distanceFromLookAt * zoomConstant;
                    float subtractValue = currentDist * subtractionRatio;

                    //Subtract part of the distance and add back part of the distance we subtracted
                    currentDist = currentDist - subtractValue + subtractValue * additiveRatio;
                    if (currentDist > distOfFurthestActorFromLookAt)
                    {
                        distOfFurthestActorFromLookAt = currentDist;
                    }
                }
            }
            return distOfFurthestActorFromLookAt;
        }

        private void ChangeCameraPosition(float distOfFurthestActorFromLookAt, GameTime gameTime)
        {
            //Move the camera farther
            Vector3 newDirection = position - lookAt;
            newDirection.Normalize();

            //Scale direction
            newDirection = newDirection * (distOfFurthestActorFromLookAt < maxDistance? distOfFurthestActorFromLookAt : maxDistance);
            newDirection = newDirection + (newDirection * minDistance);
            newDirection.Y = newDirection.Y > minHeight ? newDirection.Y < maxHeight ? newDirection.Y : maxHeight : minHeight;

            //Set new position
            Vector3 newPosition = lookAt + newDirection;

            newPosition -= position;
            float length = newPosition.Length();
            float velocity = length * distancePerUpdate;
            newPosition.Normalize();
            newPosition *= velocity * gameTime.ElapsedGameTime.Milliseconds;
            position += newPosition;

            Vector3 dir = lookAt - position;

            //Calculate Rotation
            Rotation = Quaternion.CreateFromYawPitchRoll((float)Math.Atan2(dir.X, dir.Z), (float)Math.Atan2(dir.Y, dir.Z), 0.0f);

            //Push camera a bit to the side if the player is directly under the camera
            if (Math.Floor(position.X) == Math.Floor(lookAt.X) && Math.Floor(position.Z) == Math.Floor(lookAt.Z))
            {
                position.X = position.X + 1.0f;
            }
        }
    }
}
