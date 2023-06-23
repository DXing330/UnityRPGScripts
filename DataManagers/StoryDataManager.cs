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
    protected int current_payment = 1;
    protected int current_deadline = 90;

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
        if (current_deadline < 90)
        {
            current_deadline = 90;
        }
    }

    public string ReturnPaymentDeadline()
    {
        string paydead = "";
        paydead += current_payment.ToString()+"|";
        paydead += current_deadline.ToString();
        return paydead;
    }

    public void PayBlood()
    {
        if (current_payment > 0 && GameManager.instance.villages.collected_blood > 0)
        {
            current_payment--;
            GameManager.instance.villages.collected_blood--;
        }
    }

    public void CheckTime()
    {
        if (GameManager.instance.current_day >= current_deadline)
        {
            if (CheckPayment())
            {
                Success();
            }
            else
            {
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
        current_deadline += 90;
        current_payment += 1;
        trust++;
        chapter_page++;
        if (chapter_page > story_chapter)
        {
            story_chapter++;
            chapter_page--;
            //"Onto the next chapter of the story."
        }
        //"Good_job.jpg"
    }

    protected void Fail()
    {
        current_deadline += 90;
        current_payment += current_payment;
        trust--;
        if (trust <= 0)
        {
            // You die.
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
                GameManager.instance.ShowFixedText(name, words);
                chapter_page++;
            }
        }
    }
}