
fetch:
	wget http://www.pullenti.ru/DownloadFile.aspx?file=DemoNerCore.zip -O DemoNerCore.zip
	rm -rf EP.SdkCore EP.DemoCore DemoNerCore.sln
	unzip DemoNerCore.zip
	rm DemoNerCore.zip
