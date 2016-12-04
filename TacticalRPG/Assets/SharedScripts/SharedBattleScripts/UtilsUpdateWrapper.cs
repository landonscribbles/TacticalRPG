using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UtilsUpdateWrapper : MonoBehaviour {

    public List<UtilsUpdateInterface> updateUtils;

    void Awake() {
        updateUtils = new List<UtilsUpdateInterface>();
    }

    void Update() {
        foreach (UtilsUpdateInterface updateUtil in updateUtils) {
            updateUtil.Update();
        }
    }

    public void RegisterUpdateUtil(UtilsUpdateInterface updateUtil) {
        updateUtils.Add(updateUtil);
    }
}
