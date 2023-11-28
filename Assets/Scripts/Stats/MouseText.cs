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
                SkillText.text = "Kỹ năng Q: Bắn ra một đường khí hình cung\n Gây 100% sức mạnh lên boss\n Thời gian hồi chiêu: 4,5 giây.\n Combo EQ: Dịch chuyển ngược với hướng của vị trí chuột chỉ định rồi bắn ra 3 đường kiếm khí.";
                break;
            case "Skill_W":
                ShowPannel(1);
                SkillText.text = "Kỹ năng W: Tạo ra vòng lửa bảo hộ nhân vật\n Giúp giảm 50% sát thương \n Thời gian tác dụng: 5 giây.";
                break;
            case "Skill_E":
                ShowPannel(1);
                SkillText.text = "Kỹ năng E: Lướt theo hướng chỉ định khi chạm boss thì dừng lại và gây sát thương bằng 150% sức mạnh\n Thời gian hồi chiêu: 4,5 giây.\n Combo QE: Lướt theo hướng chỉ định, đi xuyên vật thể, không bị đẩy, không dừng lại nếu đụng boss và sát thương nếu trúng vào boss.";
                break;
            case "Skill_R":
               ShowPannel(1);
                SkillText.text = "Kỹ năng R: Tăng 50% sức mạnh và đánh dấu Boss\n Sau khi hết thời gian hiệu ứng thì phát nổ gây sát thương bằng 50% tổng sát thương boss đã nhận trong thời gian tác dụng.";
                break;
            case "QLevel1":
               ShowPannel(2);
                LevelText.text = "Giảm thời gian hồi chiêu còn 3,5 giây";
                break;
            case "QLevel2":
                ShowPannel(2);
                LevelText.text = "Giảm thời gian hồi chiêu còn 2,5 giây";
                break;
            case "QLevel3":
                ShowPannel(2);
                LevelText.text = "Giảm thời gian hồi chiêu còn 1,5 giây";
                break;
            case "WLevel1":
                ShowPannel(2);
                LevelText.text = "Tăng thời gian tác dụng lên 6 giây";
                break;
            case "WLevel2":
                ShowPannel(2);
                LevelText.text = "Tăng thời gian tác dụng lên 7 giây";
                break;
            case "WLevel3":
                ShowPannel(2);
                LevelText.text = "Tăng thời gian tác dụng lên 8 giây";
                break;
            case "ELevel1":
                ShowPannel(2);
                LevelText.text = "Giảm thời gian hồi chiêu còn 3,5 giây";
                break;
            case "ELevel2":
                ShowPannel(2);
                LevelText.text = "Giảm thời gian hồi chiêu còn 2,5 giây";
                break;
            case "ELevel3":
                ShowPannel(2);
                LevelText.text = "Giảm thời gian hồi chiêu còn 1,5 giây";
                break;
            case "Slot_Item_Potion":
                ShowPannel(2);
                LevelText.text = "Bình máu: Hồi toàn bộ máu đã mất";
                break;
            case "Slot_Item_Mana":
                ShowPannel(2);
                LevelText.text = "Bình mana: Hồi toàn bộ mana đã mất";
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
                LevelText.text = "Tăng lượng Sinh Lực tối đa";
                break;
            case "MP":
                ShowPannel(2);
                LevelText.text = "Tăng lượng Linh Lực tối đa";
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
