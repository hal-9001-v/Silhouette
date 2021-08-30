using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pocket : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Money _coinPrefab;
    [SerializeField] Collectable _itemPrefab;

    [Header("Settings")]
    [SerializeField] [Range(0, 20)] int _coins;
    [SerializeField] [Range(1, 4)] int _coinsPerPoke = 4;

    public bool isEmpty { get; private set; }
    public bool canBePoked = true;

    int _pokedCoins;

    public void Poke(Inventory inventory)
    {
        if (_pokedCoins < _coins)
        {

            int coinsToSpawn = Random.Range(1, _coinsPerPoke);
            _pokedCoins += coinsToSpawn;

            if (_coinPrefab)
            {
                for (int i = 0; i < coinsToSpawn; i++)
                {
                    var coin = Instantiate(_coinPrefab);
                    coin.transform.position = transform.position;

                    coin.GetComponent<Collectable>().Collect(inventory);
                }
            }
        }
        else
        {

            if (_itemPrefab)
            {
                var item = Instantiate(_itemPrefab);
                item.transform.position = transform.position;

                item.Collect(inventory);
            }

            isEmpty = true;
        }
    }
}
