using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wasteCollection : MonoBehaviour
{

    [Header("==scoring elements===")]
    public int correctpoint;
    public int partiallycorrectpoint, wrongpoint;
    public Zonehandler zonewaste;
    public bool check = false;
    private bool ismoving;
    private RaycastHit2D hit;
    private Vector2 mousepos;
    private Vector2 initialpos;
    private bool canblast = false, isfirst = false;
    private bool reduce = false, reuse = false, recycle=false;
    public Vector2 collidersize;
    public GameObject collidedDustbin;
    public AudioClip correct_clip, wrong_clip;
    private StartpageController startpage;

    public void Start()
    {
       // startpage = FindObjectOfType<StartpageController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenpt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousepos = new Vector2(screenpt.x, screenpt.y);
            hit = Physics2D.Raycast(mousepos, Vector2.zero);
           
            if (hit.collider.tag == "waste")
            {
                hit.transform.SetAsLastSibling();
                initialpos = hit.transform.gameObject.GetComponent<RectTransform>().localPosition;
                if(hit.collider.gameObject.name == this.gameObject.name)
                {
                    hit.collider.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    collidersize = hit.collider.gameObject.GetComponent<BoxCollider2D>().size;
                    hit.collider.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(30, 30);
                }
                ismoving = true;

            }

            if(hit.collider.tag == "reduce")
            {
                hit.transform.parent.gameObject.transform.SetAsLastSibling();
                if (hit.collider.gameObject.name == this.gameObject.name)
                {
                    collidersize = hit.collider.gameObject.GetComponent<BoxCollider2D>().size;
                    hit.collider.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(30, 30);
                }
                hit.collider.gameObject.transform.GetComponentInParent<Image>().enabled = false;
                initialpos = hit.transform.gameObject.GetComponent<RectTransform>().localPosition;

                hit.collider.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                hit.collider.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                ismoving = true;
            }

            if(hit.collider.tag == "animatorobj")
            {
                hit.transform.parent.gameObject.transform.SetAsLastSibling();
                if (hit.collider.gameObject.name == this.gameObject.name)
                {
                    collidersize = hit.collider.gameObject.GetComponent<BoxCollider2D>().size;
                    hit.collider.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(30, 30);
                }

                hit.collider.gameObject.transform.GetComponentInParent<Animator>().enabled = false;
                initialpos = hit.transform.gameObject.GetComponent<RectTransform>().localPosition;
                hit.collider.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                hit.collider.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                ismoving = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            ismoving = false;
            if (!canblast)
            {
                if(hit.collider.tag == "reduce")
                {
                    if(hit.collider.gameObject.name == this.gameObject.name)
                    {
                        hit.collider.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(collidersize.x, collidersize.y);
                        hit.transform.gameObject.GetComponent<RectTransform>().localPosition = initialpos;
                        hit.collider.gameObject.transform.GetComponentInParent<Image>().enabled = true;
                        hit.collider.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        hit.collider.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
                else
                {
                    if(hit.collider.tag == "waste")
                    {
                        if (hit.collider.gameObject.name == this.gameObject.name)
                        {
                            hit.collider.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(collidersize.x, collidersize.y);
                            hit.collider.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                            hit.transform.gameObject.GetComponent<RectTransform>().localPosition = initialpos;
                        }
                    }
                    else if (hit.collider.tag == "animatorobj")
                    {
                        if (hit.collider.gameObject.name == this.gameObject.name)
                        {
                            hit.collider.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(collidersize.x, collidersize.y);
                            hit.collider.gameObject.transform.GetComponentInParent<Animator>().enabled = true;
                            hit.transform.gameObject.GetComponent<RectTransform>().localPosition = initialpos;
                            hit.collider.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                            hit.collider.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        }
                    }
                }
               
            }
            else
            {

                if (hit.collider.tag == "reduce")
                {
                    if (hit.collider.gameObject.name == this.gameObject.name)
                    {
                        hit.collider.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(collidersize.x, collidersize.y);
                        hit.collider.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (hit.collider.tag == "waste")
                    {
                        if (hit.collider.gameObject.name == this.gameObject.name)
                        {
                            hit.collider.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(collidersize.x, collidersize.y);
                            hit.collider.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        }
                    }
                    else if (hit.collider.tag == "animatorobj")
                    {
                        if (hit.collider.gameObject.name == this.gameObject.name)
                        {
                            hit.collider.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(collidersize.x, collidersize.y);
                            hit.collider.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        }
                    }
                }
                if (!check)
                {
                    check = true;
                    zonewaste.waste_count += 1;
                    if(hit.collider.tag == "reduce")
                    {
                        hit.collider.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(collidersize.x, collidersize.y);
                    }
                    if (reduce)
                    {
                        if (zonewaste.partially_reduce.Contains(this.gameObject))
                        {
                            zonewaste.level1score += partiallycorrectpoint;
                            if (zonewaste.room1_check)
                            {
                                string dashboard_data = hit.collider.gameObject.name + "," + "Reduce" + "," + 5 +",";
                                zonewaste.room1_data_collected.Add(dashboard_data);
                                zonewaste.room1_data.Add(hit.collider.gameObject.name, "Correct");
                            }
                            if (zonewaste.room2_check)
                            {
                                string dashboard_data = hit.collider.gameObject.name + "," + "Reduce" + "," + 5 + ",";
                                zonewaste.room2_data_collected.Add(dashboard_data);
                                zonewaste.room2_data.Add(hit.collider.gameObject.name, "Correct");
                            }
                            if (zonewaste.room3_check)
                            {
                                string dashboard_data = hit.collider.gameObject.name + "," + "Reduce" + "," + 5 + ",";
                                zonewaste.room3_data_collected.Add(dashboard_data);
                                zonewaste.room3_data.Add(hit.collider.gameObject.name, "Correct");
                            }
                            collidedDustbin.GetComponent<AudioSource>().clip = correct_clip;
                            collidedDustbin.GetComponent<AudioSource>().Play();
                            collidedDustbin.transform.GetChild(3).gameObject.SetActive(true);
                            collidedDustbin.transform.GetChild(3).gameObject.GetComponent<Text>().text = "+5";
                            collidedDustbin.transform.GetChild(6).gameObject.SetActive(true);
                            collidedDustbin.transform.GetChild(6).gameObject.GetComponent<Text>().text = "Nice try!";
                            collidedDustbin.transform.GetChild(4).gameObject.SetActive(true);
                            Invoke("scaledownObject", 1.5f);
                        }
                        else
                        {
                            if (zonewaste.reduce.Contains(this.gameObject))

                            {
                                zonewaste.level1score += correctpoint;
                                if (zonewaste.room1_check)
                                {
                                    string dashboard_data = hit.collider.gameObject.name + "," + "Reduce" + "," + 10 + ",";
                                    zonewaste.room1_data_collected.Add(dashboard_data);
                                    zonewaste.room1_data.Add(hit.collider.gameObject.name, "Correct");
                                }
                                if (zonewaste.room2_check)
                                {
                                    string dashboard_data = hit.collider.gameObject.name + "," + "Reduce" + "," + 10 + ",";
                                    zonewaste.room2_data_collected.Add(dashboard_data);
                                    zonewaste.room2_data.Add(hit.collider.gameObject.name, "Correct");
                                }
                                if (zonewaste.room3_check)
                                {
                                    string dashboard_data = hit.collider.gameObject.name + "," + "Reduce" + "," + 10 + ",";
                                    zonewaste.room3_data_collected.Add(dashboard_data);
                                    zonewaste.room3_data.Add(hit.collider.gameObject.name, "Correct");
                                }
                                collidedDustbin.GetComponent<AudioSource>().clip = correct_clip;
                                collidedDustbin.GetComponent<AudioSource>().Play();
                                collidedDustbin.transform.GetChild(3).gameObject.SetActive(true);
                                collidedDustbin.transform.GetChild(3).gameObject.GetComponent<Text>().text = "+10";
                                collidedDustbin.transform.GetChild(6).gameObject.SetActive(true);
                                collidedDustbin.transform.GetChild(6).gameObject.GetComponent<Text>().text = "Wow!";
                                collidedDustbin.transform.GetChild(4).gameObject.SetActive(true);
                               
                                Invoke("scaledownObject", 1.5f);
                            }
                            else
                            {
                                zonewaste.level1score -= wrongpoint;
                                if (zonewaste.room1_check)
                                {
                                    string dashboard_data = hit.collider.gameObject.name + "," + "Reduce" + "," + 0 + ",";
                                    zonewaste.room1_data_collected.Add(dashboard_data);
                                    zonewaste.room1_data.Add(hit.collider.gameObject.name, "Wrong");
                                }
                                if (zonewaste.room2_check)
                                {
                                    string dashboard_data = hit.collider.gameObject.name + "," + "Reduce" + "," + 0 + ",";
                                    zonewaste.room2_data_collected.Add(dashboard_data);
                                    zonewaste.room2_data.Add(hit.collider.gameObject.name, "Wrong");
                                }
                                if (zonewaste.room3_check)
                                {
                                    string dashboard_data = hit.collider.gameObject.name + "," + "Reduce" + "," + 0 + ",";
                                    zonewaste.room3_data_collected.Add(dashboard_data);
                                    zonewaste.room3_data.Add(hit.collider.gameObject.name, "Wrong");
                                }
                                collidedDustbin.GetComponent<AudioSource>().clip = wrong_clip;
                                collidedDustbin.GetComponent<AudioSource>().Play();
                                collidedDustbin.transform.GetChild(3).gameObject.SetActive(true);
                                collidedDustbin.transform.GetChild(3).gameObject.GetComponent<Text>().text = "+0";
                                collidedDustbin.transform.GetChild(6).gameObject.SetActive(true);
                                collidedDustbin.transform.GetChild(6).gameObject.GetComponent<Text>().text = "Ouch!";
                                collidedDustbin.transform.GetChild(5).gameObject.SetActive(true);
                               // startpage.VibrateDevice();
                                collidedDustbin.GetComponent<dustbineffect>().enabled = true;
                                Invoke("scaledownObject", 1.5f);
                            }


                        }
                    }
                    if (reuse)
                    {
                        if (zonewaste.partially_reuse.Contains(this.gameObject))
                        {
                            zonewaste.level1score += partiallycorrectpoint;
                            if (zonewaste.room1_check)
                            {
                                string dashboard_data = hit.collider.gameObject.name + "," + "Reuse" + "," + 5 + ",";
                                zonewaste.room1_data_collected.Add(dashboard_data);
                                zonewaste.room1_data.Add(hit.collider.gameObject.name, "Partially");
                            }
                            if (zonewaste.room2_check)
                            {
                                string dashboard_data = hit.collider.gameObject.name + "," + "Reuse" + "," + 5 + ",";
                                zonewaste.room2_data_collected.Add(dashboard_data);
                                zonewaste.room2_data.Add(hit.collider.gameObject.name, "Partially");
                            }
                            if (zonewaste.room3_check)
                            {
                                string dashboard_data = hit.collider.gameObject.name + "," + "Reuse" + "," + 5 + ",";
                                zonewaste.room3_data_collected.Add(dashboard_data);
                                zonewaste.room3_data.Add(hit.collider.gameObject.name, "Partially");
                            }
                            collidedDustbin.GetComponent<AudioSource>().clip = correct_clip;
                            collidedDustbin.GetComponent<AudioSource>().Play();
                            collidedDustbin.transform.GetChild(3).gameObject.SetActive(true);
                            collidedDustbin.transform.GetChild(3).gameObject.GetComponent<Text>().text = "+5";
                            collidedDustbin.transform.GetChild(6).gameObject.SetActive(true);
                            collidedDustbin.transform.GetChild(6).gameObject.GetComponent<Text>().text = "Nice try!";
                            collidedDustbin.transform.GetChild(4).gameObject.SetActive(true);
                            Invoke("scaledownObject", 1.5f);
                        }
                        else
                        {
                            if (zonewaste.reuse.Contains(this.gameObject))

                            {
                                zonewaste.level1score += correctpoint;
                                if (zonewaste.room1_check)
                                {
                                    string dashboard_data = hit.collider.gameObject.name + "," + "Reuse" + "," + 10 + ",";
                                    zonewaste.room1_data_collected.Add(dashboard_data);
                                    zonewaste.room1_data.Add(hit.collider.gameObject.name, "Correct");
                                }
                                if (zonewaste.room2_check)
                                {
                                    string dashboard_data = hit.collider.gameObject.name + "," + "Reuse" + "," + 10 + ",";
                                    zonewaste.room2_data_collected.Add(dashboard_data);
                                    zonewaste.room2_data.Add(hit.collider.gameObject.name, "Correct");
                                }
                                if (zonewaste.room3_check)
                                {
                                    string dashboard_data = hit.collider.gameObject.name + "," + "Reuse" + "," + 10 + ",";
                                    zonewaste.room3_data_collected.Add(dashboard_data);
                                    zonewaste.room3_data.Add(hit.collider.gameObject.name, "Correct");
                                }
                                collidedDustbin.GetComponent<AudioSource>().clip = correct_clip;
                                collidedDustbin.GetComponent<AudioSource>().Play();
                                collidedDustbin.transform.GetChild(3).gameObject.SetActive(true);
                                collidedDustbin.transform.GetChild(3).gameObject.GetComponent<Text>().text = "+10";
                                collidedDustbin.transform.GetChild(6).gameObject.SetActive(true);
                                collidedDustbin.transform.GetChild(6).gameObject.GetComponent<Text>().text = "Wow!";
                                collidedDustbin.transform.GetChild(4).gameObject.SetActive(true);
                                Invoke("scaledownObject", 1.5f);
                            }
                            else
                            {
                                zonewaste.level1score -= wrongpoint;
                                if (zonewaste.room1_check)
                                {
                                    string dashboard_data = hit.collider.gameObject.name + "," + "Reuse" + "," + 0 + ",";
                                    zonewaste.room1_data_collected.Add(dashboard_data);
                                    zonewaste.room1_data.Add(hit.collider.gameObject.name, "Wrong");
                                }
                                if (zonewaste.room2_check)
                                {
                                    string dashboard_data = hit.collider.gameObject.name + "," + "Reuse" + "," + 0 + ",";
                                    zonewaste.room2_data_collected.Add(dashboard_data);
                                    zonewaste.room2_data.Add(hit.collider.gameObject.name, "Wrong");
                                }
                                if (zonewaste.room3_check)
                                {
                                    string dashboard_data = hit.collider.gameObject.name + "," + "Reuse" + "," + 0 + ",";
                                    zonewaste.room3_data_collected.Add(dashboard_data);
                                    zonewaste.room3_data.Add(hit.collider.gameObject.name, "Wrong");
                                }
                                collidedDustbin.GetComponent<AudioSource>().clip = wrong_clip;
                                collidedDustbin.GetComponent<AudioSource>().Play();
                                collidedDustbin.transform.GetChild(3).gameObject.SetActive(true);
                                collidedDustbin.transform.GetChild(3).gameObject.GetComponent<Text>().text = "+0";
                                collidedDustbin.transform.GetChild(6).gameObject.SetActive(true);
                                collidedDustbin.transform.GetChild(6).gameObject.GetComponent<Text>().text = "Ouch!";
                                collidedDustbin.transform.GetChild(5).gameObject.SetActive(true);
                                //startpage.VibrateDevice();
                                collidedDustbin.GetComponent<dustbineffect>().enabled = true;
                                Invoke("scaledownObject", 1.5f);
                            }


                        }
                    }
                    if (recycle)
                    {
                        if (zonewaste.partially_recycle.Contains(this.gameObject))
                        {
                            zonewaste.level1score += partiallycorrectpoint;
                            if (zonewaste.room1_check)
                            {
                                string dashboard_data = hit.collider.gameObject.name + "," + "Recycle" + "," + 5 + ",";
                                zonewaste.room1_data_collected.Add(dashboard_data);
                                zonewaste.room1_data.Add(hit.collider.gameObject.name, "Partially");
                            }
                            if (zonewaste.room2_check)
                            {
                                string dashboard_data = hit.collider.gameObject.name + "," + "Recycle" + "," + 5 + ",";
                                zonewaste.room2_data_collected.Add(dashboard_data);
                                zonewaste.room2_data.Add(hit.collider.gameObject.name, "Partially");
                            }
                            if (zonewaste.room3_check)
                            {
                                string dashboard_data = hit.collider.gameObject.name + "," + "Recycle" + "," + 5 + ",";
                                zonewaste.room3_data_collected.Add(dashboard_data);
                                zonewaste.room3_data.Add(hit.collider.gameObject.name, "Partially");
                            }
                            collidedDustbin.GetComponent<AudioSource>().clip = correct_clip;
                            collidedDustbin.GetComponent<AudioSource>().Play();
                            collidedDustbin.transform.GetChild(3).gameObject.SetActive(true);
                            collidedDustbin.transform.GetChild(3).gameObject.GetComponent<Text>().text = "+5";
                            collidedDustbin.transform.GetChild(6).gameObject.SetActive(true);
                            collidedDustbin.transform.GetChild(6).gameObject.GetComponent<Text>().text = "Nice try!";
                            collidedDustbin.transform.GetChild(4).gameObject.SetActive(true);
                            Invoke("scaledownObject", 1.5f);
                        }
                        else
                        {
                            if (zonewaste.recycle.Contains(this.gameObject))

                            {
                                zonewaste.level1score += correctpoint;
                                if (zonewaste.room1_check)
                                {
                                    string dashboard_data = hit.collider.gameObject.name + "," + "Recycle" + "," + 10 + ",";
                                    zonewaste.room1_data_collected.Add(dashboard_data);
                                    zonewaste.room1_data.Add(hit.collider.gameObject.name, "Correct");
                                }
                                if (zonewaste.room2_check)
                                {
                                    string dashboard_data = hit.collider.gameObject.name + "," + "Recycle" + "," + 10 + ",";
                                    zonewaste.room2_data_collected.Add(dashboard_data);
                                    zonewaste.room2_data.Add(hit.collider.gameObject.name, "Correct");
                                }
                                if (zonewaste.room3_check)
                                {
                                    string dashboard_data = hit.collider.gameObject.name + "," + "Recycle" + "," + 10 + ",";
                                    zonewaste.room3_data_collected.Add(dashboard_data);
                                    zonewaste.room3_data.Add(hit.collider.gameObject.name, "Correct");
                                }
                                collidedDustbin.GetComponent<AudioSource>().clip = correct_clip;
                                collidedDustbin.GetComponent<AudioSource>().Play();
                                collidedDustbin.transform.GetChild(3).gameObject.SetActive(true);
                                collidedDustbin.transform.GetChild(3).gameObject.GetComponent<Text>().text = "+10";
                                collidedDustbin.transform.GetChild(6).gameObject.SetActive(true);
                                collidedDustbin.transform.GetChild(6).gameObject.GetComponent<Text>().text = "Wow!";
                                collidedDustbin.transform.GetChild(4).gameObject.SetActive(true);
                                Invoke("scaledownObject", 1.5f);
                            }
                            else
                            {
                                zonewaste.level1score -= wrongpoint;
                                if (zonewaste.room1_check)
                                {
                                    string dashboard_data = hit.collider.gameObject.name + "," + "Recycle" + "," + 0 + ",";
                                    zonewaste.room1_data_collected.Add(dashboard_data);
                                    zonewaste.room1_data.Add(hit.collider.gameObject.name, "Wrong");
                                }
                                if (zonewaste.room2_check)
                                {
                                    string dashboard_data = hit.collider.gameObject.name + "," + "Recycle" + "," + 0 + ",";
                                    zonewaste.room2_data_collected.Add(dashboard_data);
                                    zonewaste.room2_data.Add(hit.collider.gameObject.name, "Wrong");
                                }
                                if (zonewaste.room3_check)
                                {
                                    string dashboard_data = hit.collider.gameObject.name + "," + "Recycle" + "," + 0 + ",";
                                    zonewaste.room3_data_collected.Add(dashboard_data);
                                    zonewaste.room3_data.Add(hit.collider.gameObject.name, "Wrong");
                                }
                                collidedDustbin.GetComponent<AudioSource>().clip = wrong_clip;
                                collidedDustbin.GetComponent<AudioSource>().Play();
                                collidedDustbin.transform.GetChild(3).gameObject.SetActive(true);
                                collidedDustbin.transform.GetChild(3).gameObject.GetComponent<Text>().text = "+0";
                                collidedDustbin.transform.GetChild(6).gameObject.SetActive(true);
                                collidedDustbin.transform.GetChild(6).gameObject.GetComponent<Text>().text = "Ouch!";
                                collidedDustbin.transform.GetChild(5).gameObject.SetActive(true);
                                collidedDustbin.GetComponent<dustbineffect>().enabled = true;
                               // startpage.VibrateDevice();
                                Invoke("scaledownObject", 1.5f);
                            }


                        }
                    }
                }
                zonewaste.collected_count += 1;
                zonewaste.score_check = true;
                this.gameObject.SetActive(false);
            }

        }

        if (ismoving)
        {
            Vector2 targetpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit.transform.gameObject.GetComponent<RectTransform>().position = new Vector3(targetpos.x, targetpos.y, 0f);
        }
    }

   

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "reduce")
        {
            canblast = true;
            reduce = true;
            collidedDustbin = other.gameObject;
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            other.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            //other.gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, 50f);
        }
        if (other.gameObject.name == "reuse")
        {
            canblast = true;
            reuse = true;
            collidedDustbin = other.gameObject;
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            other.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            //other.gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, 50f);
        }
        if (other.gameObject.name == "recycle")
        {
            canblast = true;
            recycle = true;
            collidedDustbin = other.gameObject;
            other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            other.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            //other.gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, 50f);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "reduce")
        {
            canblast = false;
            reduce = false;
            //collidedDustbin = null;
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            other.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            //other.gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, -10f);
        }
        if (other.gameObject.name == "reuse")
        {
            canblast = false;
            reuse = false;
            //collidedDustbin = null;
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            other.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            //other.gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, -10f);
        }
        if (other.gameObject.name == "recycle")
        {
            canblast = false;
            recycle = false;
            //collidedDustbin = null;
            other.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            other.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            //other.gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, -10f);
        }
    }


    void scaledownObject()
    {
        Debug.Log(collidedDustbin.name);
        collidedDustbin.transform.GetChild(3).GetComponent<Text>().text = "";
        collidedDustbin.transform.GetChild(3).gameObject.SetActive(false);
        collidedDustbin.transform.GetChild(6).gameObject.GetComponent<Text>().text = "";
        collidedDustbin.transform.GetChild(6).gameObject.SetActive(false);
        collidedDustbin.GetComponent<dustbineffect>().enabled = false;
    }
}
