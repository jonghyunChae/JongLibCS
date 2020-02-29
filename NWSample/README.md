1. executer 
- 클래스와 .net core 프로세서를 실행할 수 있도록 도와주는 dll 라이브러리
- client, server 각 자 이걸 이용해서 빠른 테스트를 할 수 있도록 돕습니다.

2. servicehost 
- .net framework (.exe로 자동 빌드 되도록)
- client.dll + server.dll을 멀티 프로세서로 실행 시켜줍니다.
- (일일이 키기 귀찮으므로)
- 필요한 경우 개수를 client, server 프로세스 수를 조절하여 킬 수도 있습니다. 
- 실행시킬 클래스 이름( ex:Sample0)를 넘겨주면 자동으로 실행

3. client, server
- 실제로 코드 작성할 장소
- .net core
- 실제 작동시킬 클래스 이름를 외부(servicehost)로부터 받아 실행 시킵니다
- 함수는 기본적으로 Run 함수로 정의시켜야 함
