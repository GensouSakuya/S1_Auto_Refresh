using System.Collections.Generic;

namespace SimpleForm
{
    public class QuestionModel
    {
        public int ID { get; set; }
        public string Text { get; set; }

        public QuestionModel(int id, string text)
        {
            ID = id;
            Text = text;
        }

        public static List<QuestionModel> AllQuestions()
        {
            return new List<QuestionModel>
            {
                new QuestionModel(0, "安全提问(未设置请忽略)"),
                new QuestionModel(1, "母亲的名字"),
                new QuestionModel(2, "爷爷的名字"),
                new QuestionModel(3, "父亲出生的城市"),
                new QuestionModel(4, "您其中一位老师的名字"),
                new QuestionModel(5, "您个人计算机的型号"),
                new QuestionModel(6, "您最喜欢的餐馆名称"),
                new QuestionModel(7, "驾驶执照最后四位数字")
            };
        }
    }
}
