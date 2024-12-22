using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameObject[] viborYdara_list_;
    public GameObject[] target_list_;
    public GameObject[] otbit_list_;
    public GameObject ball;
    public float force = 5;
    int m = 0;

    public Menu menu;

    public Text txt_goalPromah;

    bool isResetBall;

    void Start()
    {
        menu.GetComponent<Menu>();

        isResetBall = false;
    }

    void Update()
    {
        if (isResetBall)
        {
            ResetBall();
            isResetBall = false;
        }
    }

    IEnumerator Cor_ResetBall()
    {
        yield return new WaitForSeconds(1);
    }

    public void ResetBall()
    {

        ball.GetComponent<Rigidbody>().angularDrag = 100;
        ball.transform.position = new Vector3(-128.304f, 2.351f, 0.028f);

    }

    


    IEnumerator Cor_CheckGoal()
    {
        yield return new WaitForSeconds(1);

        txt_goalPromah.gameObject.SetActive(true);

        // Invoke("Reset", 2f);
        StartCoroutine(Cor_ResetBall());

        menu.money += m;
        txt_goalPromah.gameObject.SetActive(false);

        if (menu.ball == 0) menu._menu_naPole_nullBall.SetActive(true);

        for (int i = 0; i < viborYdara_list_.Length; i++)
        {
            viborYdara_list_[i].GetComponent<MeshRenderer>().enabled = true;
            otbit_list_[i].SetActive(false);
        }

    }

    IEnumerator Cor_Wait()
    {
        yield return new WaitForSeconds(1);

        txt_goalPromah.gameObject.SetActive(true);

        yield return new WaitForSeconds(2);

        isResetBall = true;

        ball.GetComponent<Rigidbody>().angularDrag = 100;
        ball.transform.position = new Vector3(-128.304f, 2.351f, 0.028f);

        txt_goalPromah.gameObject.SetActive(false);
        menu.money += m;

        if (menu.ball == 0) menu._menu_naPole_nullBall.SetActive(true);

        for (int i = 0; i < viborYdara_list_.Length; i++)
        {
            viborYdara_list_[i].GetComponent<MeshRenderer>().enabled = true;
            otbit_list_[i].SetActive(false);
        }
    }

    void CheckGoal(int v, int r)
    {
        if (v != r) // забил гол
        {
            m = Random.Range(0, 51);
            txt_goalPromah.text = "ГОЛ\n+ <color=purple>" + m + "</color> монет";

        }
        else // НЕ забил
        {
            txt_goalPromah.text = "ПРОМАХ";
        }

        Invoke("ResetBall", 3f);
        StartCoroutine(Cor_Wait());
    }

    void Shoot(int v, int r)
    {
        menu.ball--;
        ball.GetComponent<Rigidbody>().angularDrag = 2;

        switch (v)
        {
            case 1:
                Vector3 shoot_1 = (target_list_[0].transform.position - ball.transform.position).normalized;
                ball.GetComponent<Rigidbody>().AddForce(shoot_1 * force, ForceMode.Impulse);
                break;
            case 2:
                Vector3 shoot_2 = (target_list_[1].transform.position - ball.transform.position).normalized;
                ball.GetComponent<Rigidbody>().AddForce(shoot_2 * force, ForceMode.Impulse);
                break;
            case 3:
                Vector3 shoot_3 = (target_list_[2].transform.position - ball.transform.position).normalized;
                ball.GetComponent<Rigidbody>().AddForce(shoot_3 * force, ForceMode.Impulse);
                break;
            case 4:
                Vector3 shoot_4 = (target_list_[3].transform.position - ball.transform.position).normalized;
                ball.GetComponent<Rigidbody>().AddForce(shoot_4 * force, ForceMode.Impulse);
                break;

            default: break;
        }

        CheckGoal(v - 1, r);
    }

    private void OnMouseUp()
    {
        if (viborYdara_list_[0].GetComponent<MeshRenderer>().enabled == true)
        { 
            if (menu.ball > 0 && menu._menu_naPole.activeInHierarchy)
            {
                int v = 0;
                gameObject.GetComponent<MeshRenderer>().enabled = false;

                for (int i = 0; i < viborYdara_list_.Length; i++)
                    if (!viborYdara_list_[i].GetComponent<MeshRenderer>().enabled) v = i;

                for (int i = 0; i < viborYdara_list_.Length; i++)
                    viborYdara_list_[i].GetComponent<MeshRenderer>().enabled = false;

                int r = Random.Range(0, 4);
                otbit_list_[r].SetActive(true);

                Shoot(v + 1, r);
            }
        }

    }

    void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.5f);

    }
    void OnMouseExit()
    {

        GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f);

    }
}
