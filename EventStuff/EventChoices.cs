using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventChoices : DialogTree
{
    protected string raw_data;
    public EventOutcomes outcomes;
    // Based on your choices you get a publically known effect.
    public List<string> result_1_quality;
    public List<string> result_2_quality;
    public List<string> result_3_quality;
    public List<string> result_1_quantity;
    public List<string> result_2_quantity;
    public List<string> result_3_quantity;
    // There is also a hidden effect that may occur.
    public List<string> hidden_1_quality;
    public List<string> hidden_2_quality;
    public List<string> hidden_3_quality;
    public List<string> hidden_1_quantity;
    public List<string> hidden_2_quantity;
    public List<string> hidden_3_quantity;
    public List<string> hidden_1_prob;
    public List<string> hidden_2_prob;
    public List<string> hidden_3_prob;

    public virtual void UpdateRawDataFromString(string file_path)
    {
        if (File.Exists(file_path))
        {
            raw_data = File.ReadAllText(file_path);
        }
    }

    protected virtual void UpdateSelfFromRaw()
    {
        string[] loaded_data_blocks = raw_data.Split("#");
        words = loaded_data_blocks[0];
        speaker_name = loaded_data_blocks[1];
        choice_1_text = loaded_data_blocks[2];
        choice_2_text = loaded_data_blocks[3];
        choice_3_text = loaded_data_blocks[4];
        result_1_quality = loaded_data_blocks[5].Split("|").ToList();
        result_2_quality = loaded_data_blocks[6].Split("|").ToList();
        result_3_quality = loaded_data_blocks[7].Split("|").ToList();
        result_1_quantity = loaded_data_blocks[8].Split("|").ToList();
        result_2_quantity = loaded_data_blocks[9].Split("|").ToList();
        result_3_quantity = loaded_data_blocks[10].Split("|").ToList();
        hidden_1_quality = loaded_data_blocks[11].Split("|").ToList();
        hidden_2_quality = loaded_data_blocks[12].Split("|").ToList();
        hidden_3_quality = loaded_data_blocks[13].Split("|").ToList();
        hidden_1_quantity = loaded_data_blocks[14].Split("|").ToList();
        hidden_2_quantity = loaded_data_blocks[15].Split("|").ToList();
        hidden_3_quantity = loaded_data_blocks[16].Split("|").ToList();
        hidden_1_prob = loaded_data_blocks[17].Split("|").ToList();
        hidden_2_prob = loaded_data_blocks[18].Split("|").ToList();
        hidden_3_prob = loaded_data_blocks[19].Split("|").ToList();
    }

    public override void ReceiveChoice(int choice)
    {
        outcomes.SetChoices(this);
        outcomes.SelectChoice(choice);
    }

    public override void Interact()
    {
        GameManager.instance.SetEvent(this);
    }
}
