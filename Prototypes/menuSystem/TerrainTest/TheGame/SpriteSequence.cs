﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheGame
{
    class SpriteSequence
    {
        /// <summary>
        /// Title of this sequence.
        /// </summary>
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// Row to reference on sprite sheet.
        /// </summary>
        private int sheetRow;
        public int SheetRow
        {
            get { return sheetRow; }
            set { sheetRow = value; }
        }

        /// <summary>
        /// Starting frame for this sequence.
        /// </summary>
        private int startFrame;
        public int StartFrame
        {
            get { return startFrame; }
            set { startFrame = value; }
        }

        /// <summary>
        /// Last frame for this sequence.
        /// </summary>
        private int endFrame;
        public int EndFrame
        {
            get { return endFrame; }
            set { endFrame = value; }
        }

        /// <summary>
        /// Whether or not this sequence should loop on completion.
        /// </summary>
        private Boolean isLoop;
        public Boolean IsLoop
        {
            get { return isLoop; }
            set { isLoop = value; }
        }

        /// <summary>
        /// Whether or not this sequence may be interrupted.
        /// </summary>
        private Boolean isInterruptable;
        public Boolean IsInterruptable
        {
            get { return isInterruptable; }
            set { isInterruptable = value; }
        }

        /// <summary>
        /// Orientation of sprite sequence.
        /// </summary>
        private Orientation orientation;
        public Orientation Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        /// <summary>
        /// Travel speed of sprite during this sequence.
        /// </summary>
        private float speed;
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public SpriteSequence() { }

        public SpriteSequence(string title, Orientation orientation, float speed, int sheetRow, int startFrame, int endFrame, Boolean loop, Boolean interruptable)
        {
            this.title = title;
            this.orientation = orientation;
            this.speed = speed;
            this.sheetRow = sheetRow;
            this.startFrame = startFrame;
            this.endFrame = endFrame;
            this.isLoop = loop;
            this.isInterruptable = interruptable;
        }

        public SpriteSequence(int sheetRow, int startFrame, int endFrame, Boolean loop)
        {
            this.sheetRow = sheetRow;
            this.startFrame = startFrame;
            this.endFrame = endFrame;
            this.isLoop = loop;
        }
    }
}
