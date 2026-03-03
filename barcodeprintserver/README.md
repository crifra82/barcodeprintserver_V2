Questa versione di barcode viene usata sia da macro che da sate
quel che cambia è il template
da sistemare e mettere in config in ahr


Dentro al file
barcodesrv.exe.config
mettere il nome stampante a cui viene data la precedenza
<setting name="Printer" serializeAs="String">
   <value>Apix 240T_2</value>
</setting>
in caso questo nome sia vuoto verrà presa la stampante impostata in configurazione di AHR

lasciare la cartella folder to watch uguale alla cartella di stampa messa in configurazione di ahr
 <setting name="FolderToWatch" serializeAs="String">
   <value>c:\barcode</value>
</setting>

Nella configurazione di ahr pers\etichette\configurazione ricordarsi di inserire
nome stampante ( secondario )
nome template  tpl_eti.xml
e i percorsi di creazione etichetta e stampa ( le stesse cartelle vanno create sul cliente )
\\tsclient\c\barcode\      
\\tsclient\c\etichette\