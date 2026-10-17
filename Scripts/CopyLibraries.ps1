robocopy "..\..\GameFrame\Deployment\Libraries\Current\netstandard2.1" "..\Code\ldjam60\Assets\Resources\Libraries" GameFrame*.dll /s /NFL /NDL /NJH /NJS /nc /ns /np

Write-Host "Files copied successfully ..."
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")