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
        //0f���o�ߎ��Ԃɂ���
        float elapsedTime = 0.0f;
        var scoreText = GetComponent<Text>();
        //time���O�ɂȂ�܂Ń��[�v������
        while (elapsedTime < time)
        {
            float rate = elapsedTime / time;
            // �e�L�X�g�̍X�V
            scoreText.text = (befor + (after - befor) * rate).ToString("f0");

            elapsedTime += Time.deltaTime;
            // 0.01�b�҂�
            yield return new WaitForSeconds(0.01f);
        }
        // �ŏI�I�Ȓ��n�̃X�R�A
        scoreText.text = after.ToString();
    }
}
