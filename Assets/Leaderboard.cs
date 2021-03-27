﻿using System;
using System.Text;
using Photon.Pun;
using TMPro;
using UnityEngine;


public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI label = default;
    private StringBuilder builder;
    private float elapsedTime;

    // Start is called before the first frame update
    void Start()
    {
        builder = new StringBuilder();
        elapsedTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //まだルームうに参加していない場合は更新しない
        if (!PhotonNetwork.InRoom) { return; }

        //0.1秒ごとにテキストを更新する
        elapsedTime += Time.deltaTime;
        if(elapsedTime > 0.1f)
        {
            elapsedTime = 0f;
            UpdateLabel();
        }
    }

    private void UpdateLabel()
    {
        var players = PhotonNetwork.PlayerList;
        Array.Sort(
            players,
            (p1, p2) =>
            {
                //スコアが多い順にソートする
                int diff = p2.GetScore() - p1.GetScore();
                if (diff != 0)
                {
                    return diff;
                }
                //スコアが同じだった場合には、IDが小さい順にソートする
                return p1.ActorNumber - p2.ActorNumber;
            }
     );

        builder.Clear();
        foreach (var player in players)
        {
            builder.AppendLine($"{player.NickName}({player.ActorNumber}) - {player.GetScore()}");
        }
        label.text = builder.ToString();
    }
}
