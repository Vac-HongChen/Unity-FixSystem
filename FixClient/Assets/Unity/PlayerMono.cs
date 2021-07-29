using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FixSystem;
public class PlayerMono : MonoBehaviour
{
    private PlayerEntity entity = new PlayerEntity(new World());
    // Start is called before the first frame update
    void Start()
    {
        var position = transform.position;
        entity.tranform.position = new FixVector2((Fix64)position.x, (Fix64)position.y);
    }

    // Update is called once per frame
    void Update()
    {
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        entity.input.clickPostion = new FixVector2((Fix64)position.x, (Fix64)position.y);

        entity.Update((Fix64)Time.deltaTime);

        transform.position = new Vector3((float)entity.tranform.position.x, (float)entity.tranform.position.y);

        print(entity.tranform.position);
        print(entity.input.clickPostion);
    }
}
