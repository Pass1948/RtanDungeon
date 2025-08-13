RtanDungeon 프로젝트 소개
이 프로젝트는 Unity로 개발된 RtanDungeon 게임의 샘플입니다. 플레이어는 먼저 서바이벌 맵에서 기본 조작을 익히고, 이어서 점프 맵으로 이동해 다양한 장애물을 넘으며 목표 지점까지 도달해야 합니다. 게임에는 외부 에셋을 사용하는 요소가 포함되어 있어 실행 전 참고해야 할 사항도 함께 안내합니다.

서바이벌 맵
프로젝트를 실행하면 처음에는 서바이벌 맵에서 게임이 시작됩니다. 이 맵에서 플레이어는 다음과 같은 활동을 체험할 수 있습니다:

아이템 채집과 무기 장착 – 맵 곳곳에 떨어져 있는 아이템을 주워 무기로 장착하고 사용할 수 있습니다. 총기와 근접 무기가 지원되며, 공격을 통해 기본 전투 시스템을 익힐 수 있습니다.

적 NPC(곰) 경계 – 서바이벌 맵에는 곰 NPC가 일정한 구역을 순찰합니다. 플레이어가 특정 범위에 접근하면 곰이 추격해 공격합니다. 저장소에는 NPC 제어를 위한 애니메이션 컨트롤러가 포함되어 있어 NPC가 존재함을 알 수 있습니다.

플레이어 스테이터스 UI – 화면 우측 하단에는 체력, 허기, 스테미너 게이지가 표시되어 서바이벌 요소를 강조합니다. 스테미너가 부족하면 이동 속도가 느려지는 등 리소스 관리가 필요합니다.

서바이벌 맵에는 워프 포털이 배치되어 있으며, 포털에 들어가면 점프 맵으로 이동할 수 있습니다.

점프 맵 시작하기
서바이벌 맵에서 특정 구역에 있는 워프 포털로 들어가면 점프 맵이 로드됩니다. 포털은 배경과 구분되는 디자인으로 눈에 띄게 배치되어 있으며, 플레이어가 접근하면 자동으로 전환됩니다.

점프 맵 조작법
점프 맵에서는 다음 입력으로 캐릭터를 조작합니다:

스페이스바 – 점프: 지면 위에서 스페이스바를 누르면 JumpPlayerCotroller가 위쪽 힘을 가해 점프하게 됩니다.

Tab 키 – 시점 전환: Tab을 누르면 3인칭과 1인칭 카메라가 토글되어 두 시점 간에 빠르게 전환할 수 있습니다.

마우스 왼쪽 버튼 – 난간 잡기: 난간이나 벽에 가까이 있을 때 좌클릭을 누르면 LedgeClimber 컴포넌트가 가까운 난간을 감지하고 플레이어를 부드럽게 위로 끌어올립니다.

점프 맵 목표와 요소
점프 맵은 다양한 높이의 플랫폼과 점프 베드가 배치된 코스입니다. 플레이어는 다음 요소를 활용하여 목표 지점까지 이동합니다:
<img width="400" height="350" alt="image" src="https://github.com/user-attachments/assets/2ac0cbe3-eb81-4987-b490-345ab614a6a3" />

점프 베드 – 침대 모양의 오브젝트는 트램펄린 역할을 합니다. 플레이어가 충돌하면 JumpBed의 이벤트가 호출되어 캐릭터를 위로 튕겨 올립니다.

난간 잡기 – 높은 벽이나 구조물에 매달려 올라갈 수 있는 파쿠르 기능입니다. LedgeClimber는 플레이어 가슴 높이에 구체 영역을 만들어 Interactable 태그를 가진 오브젝트를 탐지하고, 좌클릭 입력이 있을 때 위로 끌어올리는 코루틴을 실행합니다.

목표 지점 – 코스의 끝에는 빨간 동그라미로 표시된 목표 지점이 있으며, 해당 지점에 도달하면 점프 맵을 클리어하게 됩니다.

외부 에셋 경로 안내
이 저장소는 용량 제한 때문에 일부 외부 에셋을 포함하지 않습니다. 프로젝트 실행 시 빈 오브젝트나 누락된 UI가 보일 수 있으므로 아래 링크에서 필요한 에셋을 다운로드해 적용하세요.

외부 에셋 다운로드 링크: Pandazole Lowpoly Asset Bundle

Unity Asset Store 무료 에셋
이 프로젝트에서 추천하는 무료 에셋은 다음과 같습니다:

에셋 이름	설명
Pandazole – 저 폴리 에셋 번들	로우폴리 맵에셋으로, 배경과 구조물을 손쉽게 꾸밀 수 있는 에셋 번들입니다.

관련 코드 및 참고 링크
아래는 본 README에서 언급한 스크립트의 핵심 부분을 보여주는 참고 코드 링크입니다.

JumpPlayerCotroller.Move: 카메라 기준으로 플레이어 이동 처리
GitHub
.

JumpPlayerCotroller.OnJump: 지면 체크 후 점프 힘 적용
GitHub
.

JumpPlayerCotroller.ToggleView: Tab 키 입력으로 1인칭/3인칭 카메라 전환
GitHub
.

JumpBed: 플레이어 충돌 시 점프 이벤트 호출
LedgeClimber: 난간 감지와 파쿠르 이동 구현
<img width="400" height="350" alt="image" src="https://github.com/user-attachments/assets/2ac0cbe3-eb81-4987-b490-345ab614a6a3" />

- 서바이벌맵에서 해당 구역에 워프로 들어가면 점프맵으로 이동이 가능합니다

  >점프맵 기능 소개
  - 스페이스바 : 점프
  - Tab키 : 시점 변경(3인칭/1인칭)
  - 마우스 왼쪽 버튼 : 난간잡고 올라가기
  <난간잡기>
<img width="400" height="350" alt="image" src="https://github.com/user-attachments/assets/d8ff4587-d712-40eb-8e69-daae7a9f90ee" />
<img width="400" height="350" alt="image" src="https://github.com/user-attachments/assets/15bc3ae4-e64b-4b66-8845-0b51165617fb" />

  점프맵 소개
<img width="400" height="350" alt="image" src="https://github.com/user-attachments/assets/79f43b13-ac49-4aa9-8ae8-479140caea09" />
- 빨간동그라미 친곳이 목표지점입니다

<img width="400" height="350" alt="image" src="https://github.com/user-attachments/assets/f767df6d-7f32-4a3b-bdc9-df8d8a7a1654" />
침대는 점프대 입니다

⚠️ 외부 에셋 경로 안내 해당 프로젝트는 용량 제한으로 인해 외부 에셋이 GitHub에 포함되어 있지 않습니다. 프로젝트 실행 시 일부 오브젝트나 UI가 나타나지 않을 수 있으니, 아래 링크를 통해 외부 에셋을 다운받아야 합니다.
📦 외부 에셋 다운로드 링크 : https://assetstore.unity.com/packages/3d/props/pandazole-lowpoly-asset-bundle-226938
✅ Unity Asset Store 무료 에셋 에셋 이름 : 설명
1. Pandazole - 저 폴리 에셋 번들 : 로우폴리 맵에셋
