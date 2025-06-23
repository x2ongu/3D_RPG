# [Unity 3D] 3D_RPG Portfolio

## 1. 소개

- 이 프로젝트는 Unity로 개발한 3D RPG 게임입니다.
  
- 개발 기간: 2024.08.05 ~ 2025.01.31(약 6개월)
  
- Repository에는 소스코드만 등록되어 있습니다.<br><br><br>


## 2. 개발 환경

- Unity 2022.3.16f1 LTS
  
- C#
  
- Window 10<br><br><br>


## 3. 기능

- **플레이어 이동**: 마우스 우클릭으로 플레이어가 클릭한 위치로 이동하도록 구현.
  
- **3단 Zoom**: Cinemachine Virtual Camera를 활용하여 3단 줌 기능 구현.
  
- **페이크 로딩**: 로딩 화면을 페이크로 구현하여 실제 로딩 시간을 감추고 시각적으로 표현.
  
- **콤보 공격**: 일정 시간 내에 마우스를 누르면 연속 공격(콤보)이 발동되며 공격 애니메이션의 특정 프레임에서 콤보 체크가 시작되고 콤보가 이어지도록 구현.
  
- **전투 모드**: 공격하거나 피격 시 전투 자세로 전환되며 10초 동안 추가적인 공격이나 피격이 없으면 전투 모드를 해제하여 칼을 집어넣는 동작을 실행.
  
- **스킬**: 퀵슬롯에 등록된 스킬을 알맞은 키로 사용하고 스킬에 맞는 애니메이션과 이동을 재생하며 사용 중에는 피격과 사망을 제외한 모든 조작이 불가능하도록 설정.
  
- **Drag&Drop UI**: UI 요소를 드래그 앤 드롭하여 자유롭게 위치 변경 가능하도록 구현.
  
- **Enemy AI**: FSM을 이용하여 적 캐릭터들이 플레이어를 추적하고 공격하는 AI 구현.
  
- **퀘스트 시스템**: Google Sheets를 활용하여 퀘스트 데이터를 관리하고 진행 상황을 추적.
  
- **저장 및 불러오기**: JsonUtility를 사용해 플레이어와 몬스터의 데이터를 저장하고 불러오는 기능 구현.<br><br><br>


## 4. 기술 스택

- **Unity**: 게임 개발 엔진
  
- **C#**: 게임 로직 및 스크립트 작성
  
- **Visual Studio**: 코드 편집기
  
- **싱글톤**: Manager.cs 등 전역에서 하나만 필요한 객체 관리
  
- **오브젝트 풀링**: 성능 최적화를 위한 객체 재사용 기법
  
- **FSM (Finite State Machine)**: 적 AI 구현 및 상태 기반 행동 처리
  
- **Google Sheets**: 퀘스트 데이터 관리
  
- **JsonUtility**: JSON 파일을 사용한 Player 및 Monster 스탯 관리
  
- **NavMesh**: AI 몬스터 및 플레이어 이동 구현
  
- **UI 자동화 시스템**: 동적 UI 요소 관리 및 효율적 바인딩
  
- **Scriptable Object**: 아이템 및 스킬 데이터 관리
  
- **Render Texture**: 미니맵 및 UI 내 실시간 렌더링
  
- **Cinemachine Virtual Camera**: 3단 줌 기능 구현
  
- **Project**: 필드 보스 공격 위험 구역 표시<br><br><br>


## 5. 설치 및 실행 방법
1. [게임 실행 파일 다운로드](https://drive.google.com/drive/folders/1vDj_LXFu0k16GR61LfcWAPkyCtxWrXD4?usp=sharing)
2. `3D_RPG_장성우_포트폴리오.zip` 파일을 압축 해제합니다.
3. `3D_RPG.exe` 파일을 실행하여 게임을 시작합니다.<br><br><br>

## 6. 영상 링크
[유튜브 바로가기](https://youtu.be/nCCeOXKd1NQ)<br><br><br>

## 7. License

This work is licensed under the Creative Commons Attribution-NonCommercial 4.0 International License. 

You may not use the material for commercial purposes. 

For more details, see: [https://creativecommons.org/licenses/by-nc/4.0/](https://creativecommons.org/licenses/by-nc/4.0/)
