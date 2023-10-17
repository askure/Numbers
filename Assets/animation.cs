using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class animation : MonoBehaviour
{
    private int befor, after;

    public void NumAnimation(int befor,int after)
    {
        this.befor = befor;
        this.after = after;
        GetComponent<Animator>().enabled = true;
    }
    public IEnumerator ScoreAnimation()
    {
        var time = 0.07f;
        //0fを経過時間にする
        float elapsedTime = 0.0f;
        var scoreText = GetComponent<Text>();
        //timeが０になるまでループさせる
        while (elapsedTime < time)
        {
            float rate = elapsedTime / time;
            // テキストの更新
            scoreText.text = (befor + (after - befor) * rate).ToString("f0");

            elapsedTime += Time.deltaTime;
            // 0.01秒待つ
            yield return new WaitForSeconds(0.01f);
        }
        // 最終的な着地のスコア
        scoreText.text = after.ToString();
    }
}
