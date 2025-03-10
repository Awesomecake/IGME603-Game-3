using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class RenderSymbols : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    private Enemy.State _currentState = Enemy.State.Normal;
    [SerializeField] private GameObject stunnedIndicatorPrefab;
    [SerializeField] private GameObject alertedIndicatorPrefab;
    [SerializeField] private GameObject investigatingIndicatorPrefab;

    [SerializeField] private Vector3 offset = Vector3.up;
    private GameObject _stunnedIndicator;
    private GameObject _alertedIndicator;
    private GameObject _investigatingIndicator;


    private void Start()
    {
        _stunnedIndicator = Instantiate(stunnedIndicatorPrefab, enemy.transform);
        _alertedIndicator = Instantiate(alertedIndicatorPrefab, enemy.transform);
        _investigatingIndicator = Instantiate(investigatingIndicatorPrefab, enemy.transform);
        EnableIndicator(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.GetComponent<Enemy>().GetState() != _currentState)
        {
            switch (enemy.GetComponent<Enemy>().GetState())
            {
                case Enemy.State.Normal:
                    EnableIndicator(null);
                    break;
                case Enemy.State.Chasing:
                    EnableIndicator(_alertedIndicator);
                    break;
                case Enemy.State.Investigating:
                    EnableIndicator(_investigatingIndicator);
                    break;
                case Enemy.State.Confused:
                    EnableIndicator(_investigatingIndicator);
                    break;
                case Enemy.State.Stunned:
                    EnableIndicator(_stunnedIndicator);
                    break;
            }
        }
        _currentState = enemy.GetComponent<Enemy>().GetState();

        
    }

    private void EnableIndicator(GameObject indicator)
    {
        _stunnedIndicator?.SetActive(false);
        _alertedIndicator?.SetActive(false);
        _investigatingIndicator?.SetActive(false);

        if (indicator != null)
        {
            indicator.transform.rotation = Quaternion.identity;
            indicator.transform.position = enemy.transform.position + offset;
            indicator.SetActive(true);
        }
    }

    private void LateUpdate()
    {

        _stunnedIndicator.transform.rotation = Quaternion.identity;
        _alertedIndicator.transform.rotation = Quaternion.identity;
        _investigatingIndicator.transform.rotation = Quaternion.identity;

    }
}
