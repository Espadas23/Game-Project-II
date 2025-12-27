using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class FogSystem
{
    public string systemName;             // Имя системы для инспектора
    public Toggle toggle;                 // Чекбокс UI
    public List<GameObject> objectsToControl; // Все объекты системы
}

public class FogSwitch : MonoBehaviour
{
    [Header("Fog of War Systems")]
    public List<FogSystem> fogSystems = new List<FogSystem>();

    void Start()
    {
        // Подписка на изменения чекбоксов
        foreach (var system in fogSystems)
        {
            if (system.toggle != null)
                system.toggle.onValueChanged.AddListener((isOn) => OnToggleChanged(system, isOn));
        }

        // Инициализация: включаем только одну систему
        InitializeSystems();
    }

    private void InitializeSystems()
    {
        bool oneActiveFound = false;

        foreach (var system in fogSystems)
        {
            bool shouldBeActive = system.toggle != null && system.toggle.isOn;

            if (shouldBeActive)
            {
                if (!oneActiveFound)
                {
                    oneActiveFound = true;
                    SetSystemActive(system, true);
                }
                else
                {
                    // Если уже была активная система, выключаем остальные
                    system.toggle.isOn = false;
                    SetSystemActive(system, false);
                }
            }
            else
            {
                SetSystemActive(system, false);
            }
        }

        // Если ни одна галочка не включена, включаем первую систему по умолчанию
        if (!oneActiveFound && fogSystems.Count > 0)
        {
            if (fogSystems[0].toggle != null)
                fogSystems[0].toggle.isOn = true;
            SetSystemActive(fogSystems[0], true);
        }
    }

    private void OnToggleChanged(FogSystem changedSystem, bool isOn)
    {
        if (isOn)
        {
            // Выключаем все остальные системы
            foreach (var system in fogSystems)
            {
                if (system != changedSystem && system.toggle != null)
                    system.toggle.isOn = false;
            }
        }

        SetSystemActive(changedSystem, isOn);
    }

    private void SetSystemActive(FogSystem system, bool active)
    {
        if (system.objectsToControl == null) return;

        foreach (var obj in system.objectsToControl)
        {
            if (obj != null)
                obj.SetActive(active);
        }
    }
}
