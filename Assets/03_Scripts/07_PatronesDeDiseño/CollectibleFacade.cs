using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollectibleFacade
{
    /// <summary>
    /// Seteo el coleccionable a su respectivo lugar en base a la lista de CollectibleFade
    /// </summary>
    /// <param name="collectible"></param>
    /// <param name="targetPlayer"></param>
    /// <returns></returns>
    public static int SetCollectible(Collectible collectible, List<CollectibleManager> collectiblesManagerList)
    {
        int buildIndex = 0;

        //Recorro cada CollectibleFaced 
        foreach (var item in collectiblesManagerList)
        {
            if (collectible.trinketCharacter == CharacterTarget.Bongo)
            {
                //Si la referencia del colecicoanble de Bongo esta vacía
                if (!item.trincketBongo)
                {
                    //Agrego la que me pasaron por parametro
                    item.trincketBongo = collectible;

                    //Y le seteo el index del CollectibleFaced
                    buildIndex = item.buildIndexLevel;
                }
            }

            else
            {
                if (!item.trincketFrank)
                {
                    item.trincketFrank = collectible;
                    buildIndex = item.buildIndexLevel;
                }
            }
        }

        return buildIndex;
    }

    /// <summary>
    /// En base al nuevo levelIndex, actualizo la UI
    /// </summary>
    /// <param name="buildIndexLevel"></param>
    public static void UpdateUICollectible(int buildIndexLevel, UICollectible UIBongoTrincket, UICollectible UIFrankTrincket)
    {
        if (!UIBongoTrincket)
        {
            Debug.LogError($"Falta la referencia de {UIBongoTrincket.name}");
            return;
        }

        if (!UIFrankTrincket)
        {
            Debug.LogError($"Falta la referencia de {UIFrankTrincket.name}");
            return;
        }

        UIBongoTrincket.SetUIToLevel(buildIndexLevel);
        UIFrankTrincket.SetUIToLevel(buildIndexLevel);
    }

}
