using System;
using UnityEngine;
using Model;
using System.Collections;

namespace Event {
    public class ColorizeAction : BasicAction {
        private GameObject os;
        private Color col;

        public ColorizeAction(Color col, GameObject os) {
            this.os = os;
            this.col = col;
        }

        public override IEnumerator Run() {

            os.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", col);

            yield break;
        }
    }
}

