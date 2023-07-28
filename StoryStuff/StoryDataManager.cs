using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryDataManager : MonoBehaviour
{
    protected string save_string = "";
    protected string load_string = "";
    // Keep track of where you are in the main story.
    protected int story_chapter = 0;
    protected int chapter_page = 0;
    // Keep track of how much they like you.
    protected int trust = 0;
    // Gotta pay up.
    protected int current_debt = 0;
    // Minimum you need to pay each time period.
    // If they notice you're a higher level they'll make you pay more.
    protected int current_payment = 1;
    protected int current_deadline = 60;

    protected void ConvertSelfToString()
    {
        save_string = "";
        save_string += story_chapter.ToString()+"|";
        save_string += chapter_page.ToString()+"|";
        save_string += trust.ToString()+"|";
        save_string += current_debt.ToString()+"|";
        save_string += current_payment.ToString()+"|";
        save_string += current_deadline.ToString();
    }

    public void SaveData()
    {
        if (!Directory.Exists("Assets/Saves/Story"))
        {
            Directory.CreateDirectory("Assets/Saves/Story");
        }
        ConvertSelfToString();
        File.WriteAllText("Assets/Saves/Story/main.txt", save_string);
    }

    public void LoadData()
    {
        if (File.Exists("Assets/Saves/Story/main.txt"))
        {
            load_string = File.ReadAllText("Assets/Saves/Story/main.txt");
            string[] loaded_data = load_string.Split("|");
            story_chapter = int.Parse(loaded_data[0]);
            chapter_page = int.Parse(loaded_data[1]);
            trust = int.Parse(loaded_data[2]);
            current_debt = int.Parse(loaded_data[3]);
            current_payment = int.Parse(loaded_data[4]);
            current_deadline = int.Parse(loaded_data[5]);
        }
        if (current_deadline < 60)
        {
            current_deadline = 60;
        }
    }

    public string ReturnPaymentDeadline()
    {
        string paydate = "";
        paydate += current_payment.ToString()+"|";
        paydate += current_deadline.ToString();
        return paydate;
    }

    public int ReturnDeadlineDate()
    {
        return current_deadline;
    }

    public void PayBlood()
    {
        if (current_payment > 0 && GameManager.instance.villages.collected_blood > 0)
        {
            current_payment--;
            GameManager.instance.villages.collected_blood--;
        }
        else if (current_payment > 0 && GameManager.instance.villages.collected_blood <= 0)
        {
            GameManager.instance.ShowInteractableText("Master we haven't collected enough blood to pay our quota yet, let's work hard to get more blood!", "Master Accountant Blaty");
        }
    }

    public void CheckTime()
    {
        if (GameManager.instance.current_day >= current_deadline)
        {
            Debug.Log("checking payment");
            if (CheckPayment())
            {
                Debug.Log("payment");
                Success();
            }
            else
            {
                Debug.Log("no payment");
                Fail();
            }
        }
    }

    public bool CheckPayment()
    {
        if (current_payment <= 0)
        {
            return true;
        }
        return false;
    }

    protected void Success()
    {
        Debug.Log("success");
        current_deadline += 60;
        current_payment += 1 + GameManager.instance.current_day/60;
        GameManager.instance.ShowInteractableText("Good job paying off your quota this time, we'll be back in a few months to check back in on you, collect "+current_payment+" blood by then.", "Big Guy");
        trust++;
        chapter_page++;
        if (chapter_page > story_chapter)
        {
            story_chapter++;
            chapter_page--;
            //"Onto the next chapter of the story."
        }
    }

    protected void Fail()
    {
        Debug.Log("failure");
        current_deadline += 60;
        current_payment += current_payment + GameManager.instance.current_day/45;
        trust--;
        if (trust <= 0)
        {
            GameManager.instance.ShowInteractableText("You're not very good at this.", "Big Guy");
            if (GameManager.instance.player.playerLevel > 1)
            {
                
                int new_level = Mathf.Max(1, GameManager.instance.player.playerLevel - current_payment);
                GameManager.instance.player.SetLevel(new_level);
                GameManager.instance.ShowInteractableText("Looks like you've had a bit too much blood to drink.  No worries, we'll help you get it out of your system. Next time make you sure finish your quota before indulging.", "Big Guy");
            }
            else
            {
                // Show some kind of game over message.
                //GameManager.instance.NewGame();
            }
        }
    }

    public void PlayStory()
    {

        if (story_chapter == 0)
        {
            if (chapter_page == 0)
            {
                string name = "Dracula";
                string words = "I've gifted you a village to get you started. I'll return in 3 months, collect some blood by then.";
                GameManager.instance.ShowInteractableText(words, name);
                chapter_page++;
            }
        }
    }

}