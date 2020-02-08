set platformName=%1
set outDir=%2
set projectDir=%3

:: 복사할 장소
set outputDir=%projectDir%output
set target=%projectDir%output\%platformName%

echo .
echo .
echo start copy_output.bat
echo args : %platformName% %outDir% %projectDir% %sdlpath%

:: 이전 결과물 삭제
echo delete before outputs
echo del /q "%target%"
del /q "%target%"		

::echo 플랫폼 폴더 생성 (x64, x84)
echo make targetpath with platform (x64, x84)
echo mkdir %target%
mkdir %target%

::echo 빌드 결과물 복사
echo copy outdir to targetpath
echo copy /Y %outDir% %target%
copy /Y %outDir% %target%

::echo 빌드 결과물을 플랫폼 상관없이 한 번 더 복사 (dll 사용 솔루션에서 참조 목적)
echo copy outdir to output folder
echo copy /Y %outDir% %outputDir%
copy /Y %outDir% %outputDir%

::echo DLL 폴더에서 SDL 라이브러리 복사
echo copy DLL Folder(SDL Lib) to targetpath
echo copy /Y %projectDir%SDLLib\%platformName%\ %target%
copy /Y %projectDir%SDLLib\%platformName%\ %target%

echo end copy_output.bat
echo .
echo .
