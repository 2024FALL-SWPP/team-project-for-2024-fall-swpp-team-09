// IInteractable.cs - 상호작용 가능한 객체들이 구현할 인터페이스
public interface IInteractable
{
    string GetInteractionPrompt();  // 상호작용 프롬프트 텍스트 반환 (예: "E키를 눌러 책상 조사하기")
    void OnInteract();  // 상호작용 시 실행될 메서드
    bool CanInteract(); // 현재 상호작용 가능한지 여부 반환
}