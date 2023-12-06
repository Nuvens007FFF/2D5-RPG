using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
public class MouseText : MonoBehaviour
{
    public GameObject panelSkill;
    public GameObject pannelLevel;
    public Text SkillText;
    public Text LevelText;

    private void Start()
    {
        HidePannel();
    }
    public void HidePannel()
    {
        pannelLevel.SetActive(false);
        panelSkill.SetActive(false);
    }
    public void ShowPannel(int number)
    {   
        pannelLevel.SetActive(number != 1);
        panelSkill.SetActive(number == 1);
    }
    private void Update()
    {
        Vector3 mousePositionScreen = Input.mousePosition;
        var mousePositionWorld = Camera.main.ScreenToWorldPoint(mousePositionScreen);
        pannelLevel.transform.position = new Vector3 (mousePositionWorld.x, mousePositionWorld.y + 1.5f, 10);

        Ray ray = Camera.main.ScreenPointToRay(mousePositionScreen);

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            GameObject objectHit = hit.collider.gameObject;
            ShowInfor(objectHit.name);
        }
        else
        {
             HidePannel();
        }
    }
    public void ShowInfor(string infor)
    {
        switch (infor)
        {
            case "Skill_Q":
                ShowPannel(1);
                SkillText.text = "<color=#FF5000>Hỏa Trảm:</color> Bắn đường kiếm khí về hướng chỉ định\n Gây sát thương bằng 100% Công Kích\n <color=#FF5000>Cường Hóa:</color> Nhảy ngược lại với hướng của vị trí chuột chỉ định rồi bắn ra 3 đường kiếm khí.\n <color=#00C1FF>Cần 1 Linh Lực</color>";
                break;
            case "Skill_W":
                ShowPannel(1);
                SkillText.text = "<color=#FF5000>Hỏa Thuẫn:</color> Tạo ra vòng lửa bảo hộ nhân vật\n Giúp giảm 50% sát thương trong 5 giây.\n <color=#00C1FF>Cần 5 Linh Lực</color>";
                break;
            case "Skill_E":
                ShowPannel(1);
                SkillText.text = "<color=#FF5000>Hỏa Bộ:</color> Lướt theo hướng chỉ định nếu vào chạm kẻ địch thì dừng lại và gây sát thương bằng 150% Công Kích\n <color=#FF5000>Cường Hóa:</color> Lướt theo hướng chỉ định, đi xuyên vật thể, không bị đẩy, không dừng lại và gây sát thương bằng 225% Công Kích nếu trúng kẻ địch.\n <color=#00C1FF>Cần 2 Linh Lực</color>";
                break;
            case "Skill_R":
               ShowPannel(1);
                SkillText.text = "<color=#FF5000>Hỏa Giới:</color> Phá hủy hầu hết đòn tấn công tầm xa trên bản đồ, thanh tẩy các hiệu ứng bất lợi, tăng 50% Công Kích, 5 Tốc Độ và tạo dấu ấn trên kẻ dịch trong 20 giây, hết thời gian sẽ phát nổ gây sát thương bằng 50% tổng sát thương đã nhận.\n <color=#FF5000>Chỉ có thể sử dụng khi đạt 100 Hỏa Năng, nhận Hỏa Năng dựa trên số đòn đánh trúng kẻ địch</color>\n<color=#FFCD00>Mở khóa cần 1000 Vàng</color>";
                break;
            case "Passive":
                ShowPannel(1);
                SkillText.text = "<color=#FF5000>Hỏa Kiếm:</color> Sau khi sử dụng một kỹ năng tấn công bất kỳ , cường hóa kỹ năng tấn công tiếp theo\n <color=#FF5000>Thời gian cường hóa:</color> 1.5 giây";
                break;
            case "QLevel1":
               ShowPannel(2);
                LevelText.text = "Giảm thời gian hồi chiêu còn 3,5 giây\n<color=#FFCD00>Cần 500 Vàng</color>";
                break;
            case "QLevel2":
                ShowPannel(2);
                LevelText.text = "Giảm thời gian hồi chiêu còn 2,5 giây\n<color=#FFCD00>Cần 500 Vàng</color>";
                break;
            case "QLevel3":
                ShowPannel(2);
                LevelText.text = "Giảm thời gian hồi chiêu còn 1,5 giây\n<color=#FFCD00>Cần 500 Vàng</color>";
                break;
            case "WLevel1":
                ShowPannel(2);
                LevelText.text = "Tăng thời gian tác dụng lên 6 giây\n<color=#FFCD00>Cần 500 Vàng</color>";
                break;
            case "WLevel2":
                ShowPannel(2);
                LevelText.text = "Tăng thời gian tác dụng lên 7 giây\n<color=#FFCD00>Cần 500 Vàng</color>";
                break;
            case "WLevel3":
                ShowPannel(2);
                LevelText.text = "Tăng thời gian tác dụng lên 8 giây\n<color=#FFCD00>Cần 500 Vàng</color>";
                break;
            case "ELevel1":
                ShowPannel(2);
                LevelText.text = "Giảm thời gian hồi chiêu còn 3,5 giây\n<color=#FFCD00>Cần 500 Vàng</color>";
                break;
            case "ELevel2":
                ShowPannel(2);
                LevelText.text = "Giảm thời gian hồi chiêu còn 2,5 giây\n<color=#FFCD00>Cần 500 Vàng</color>";
                break;
            case "ELevel3":
                ShowPannel(2);
                LevelText.text = "Giảm thời gian hồi chiêu còn 1,5 giây\n<color=#FFCD00>Cần 500 Vàng</color>";
                break;
            case "Slot_Item_Potion":
                ShowPannel(2);
                LevelText.text = "Bình máu: Hồi toàn bộ máu đã mất\n<color=#FFCD00>Cần 500 Vàng</color>";
                break;
            case "Slot_Item_Mana":
                ShowPannel(2);
                LevelText.text = "Bình mana: Hồi toàn bộ mana đã mất\n<color=#FFCD00>Cần 500 Vàng</color>";
                break;
            case "Coin Price":
                ShowPannel(2);
                LevelText.text = "Số tiền yêu cầu cho lần nâng cấp này";
                break;
            case "ATK":
                ShowPannel(2);
                LevelText.text = "Tăng sức tấn công của nhân vật";
                break;
            case "HP":
                ShowPannel(2);
                LevelText.text = "Tăng lượng Sinh Lực tối đa, là sức chống chịu của nhân vật";
                break;
            case "MP":
                ShowPannel(2);
                LevelText.text = "Tăng lượng Linh Lực tối đa, cần để sử dụng kỹ năng";
                break;
            case "Regen_MP":
                ShowPannel(2);
                LevelText.text = "Tăng tốc độ hồi Linh Lực và Hỏa Năng";
                break;
            case "AGI":
                ShowPannel(2);
                LevelText.text = "Tăng tốc độ di chuyển cho nhân vật";
                break;
            default:
                SkillText.text = null;
                break;
        }
    }
}
