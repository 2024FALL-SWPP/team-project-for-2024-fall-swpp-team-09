1. `unity/slides`에 있는 모든 에셋을 불러온다.
2. `Slide`, `Slide00`~`Slide14` 텍스처를 설정한다.
   1. `Advanced`/`Read/Write`를 `true`로 설정한다.
   2. `Default`/`Format`을 `RGBA 32 bit`로 설정한다.
   3. 변경사항을 적용한다.
3. `Slide` 머티리얼을 설정한다.
   1. `Shader`를 `Custom`/`Slide`로 설정한다.
   2. `Main Texture`를 `Slide` 텍스처로 설정한다.
4. `Slide` 오브젝트를 설정한다.
   1. 평면 오브젝트 `Slide`를 생성한다.
   2. `Transform`/`Scale`을 `X`와 `Z`가 $4:3$이 되도록 설정한다.
   3. `Mesh Renderer`/`Materials`/`Element 0`을 `Slide` 머티리얼로 설정한다.
5. `SlideController` 오브젝트를 설정한다.
   1. 빈 오브젝트 `SlideController`를 생성한다.
   2. `SlideController` 스크립트를 추가한다.
   3. `SlideController`/`Slide Texture`를 `Slide` 텍스처로 설정한다.
   4. `SlideController`/`Default Textures`에 `Slide00` 텍스처부터 `Slide14` 텍스처까지 추가한다.
6. 재생한다.
   1. `00`부터 `14`까지 입력해 슬라이드를 변경할 수 있다.
   2. `S`를 입력해 이상현상을 시작할 수 있다.
   3. `R`를 입력해 슬라이드를 초기화할 수 있다.
