using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class ObjectFactory : ScriptableObject
{
    Scene gameScene;

    protected T CreateObjectInstance<T> (T prefab) where T : MonoBehaviour{
        if(!gameScene.isLoaded){
            if(Application.isEditor){
                gameScene = SceneManager.GetSceneByName(name);
                if(!gameScene.isLoaded){
                    gameScene = SceneManager.CreateScene(name);   
                }
            }
            else{
                gameScene = SceneManager.CreateScene(name);
            }
        }
        T instance = Instantiate(prefab);
        SceneManager.MoveGameObjectToScene(instance.gameObject, gameScene);
        return instance;
    }

}