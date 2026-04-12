Imports System.Data.Common
Imports System.Drawing
Imports System.IO
Imports System.Reflection.Metadata
Imports System.Windows.Forms
Imports Google.Apis.Util
Imports Npgsql

Public Class FrmBattle
    Dim UpdateInterval As Boolean = False
    Private _PlayerNo As Integer

    Public Function GetPlayerNo() As Integer
        Return Me._PlayerNo
    End Function

    Public Sub SetPlayerNo(AutoPropertyValue As Integer)
        Me._PlayerNo = AutoPropertyValue
    End Sub

    Dim HandData As List(Of HandDataDto) = New List(Of HandDataDto)
    Dim BattleAreaAllyData As List(Of CardDataDto) = New List(Of CardDataDto)
    Dim BattleAreaEnemyData As List(Of CardDataDto) = New List(Of CardDataDto)
    Dim ReaderData As CardDataDto = New CardDataDto()
    Dim GraveData As List(Of CardDataDto) = New List(Of CardDataDto)
    Dim SelectionCard As CardDataDto = New CardDataDto()
    Dim SelectionCardE As CardDataDto = New CardDataDto()

    Dim con As NpgsqlConnection

    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        DBManager.getDBConnect(con)
        con.Open()

        AllCardData.GetData()

        For i As Integer = 1 To Constant.AllyCardNum
            BattleAreaAllyData.Add(New CardDataDto())
        Next

        Await StartUpdateTask()

    End Sub

    Private Sub DeckShuffle_Button_Click(sender As Object, e As EventArgs) Handles btShuffleButton.Click

        Dim result As MsgBoxResult = MsgBox("デッキをシャッフルしますか？", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "確認")

        If result = MsgBoxResult.No Then
            Exit Sub
        End If

        Dim deck As IList(Of IList(Of Object))
        Dim archive As IList(Of IList(Of Object))

        deck = Common.GetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_DECK, MAIN_SHEET_DECK_RANGE)

        archive = Common.ListShuffle(Of IList(Of Object))(deck)

        Common.SetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_ARCHIVE, MAIN_SHEET_ARCHIVE_CARD_NAME_RANGE(GetPlayerNo()), archive)

    End Sub

    Private Sub Deck_Click(sender As Object, e As EventArgs) Handles btDeck.Click

        If HandData.Count >= HandCardNum Then
            Exit Sub
        End If

        Dim archive As IList(Of IList(Of Object)) = Nothing

        While archive Is Nothing OrElse archive.Count = 0
            archive = Common.GetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_ARCHIVE, MAIN_SHEET_ARCHIVE_RANGE(GetPlayerNo()))
            Task.Delay(1000).Wait()
        End While


        Dim index = 1
        Dim drawIndex = 1
        Dim disCartCount = 1
        Dim isGet = False
        For Each card In archive
            If card.Count = 2 Then
                If Not isGet Then
                    HandData.Add(New HandDataDto(card(0), card(1), ""))
                    isGet = True
                    drawIndex = index
                End If
            Else
                If card(2).ToString = "1" Then
                    disCartCount += 1
                End If
            End If
            index += 1
        Next

        SetGoogleSheetData(MAIN_SHEET_ID, MAIN_SHEET_NAME_ARCHIVE, Constant.MAIN_SHEET_ARCHIVE_CARD_FRAW_FLG_COLUMN(GetPlayerNo()) & drawIndex + 1, "1")

        btDeck.Text = (MAX_DECK_COUNT - disCartCount).ToString

        HandCardUpdate()

        If HandData.Count >= HandCardNum Then
            lbHandMaxSign.Visible = True
        End If

        SetEnemyUpdateFlg()
    End Sub

    Private Sub pnCard_Click(sender As Object, e As MouseEventArgs) Handles pnHand1.MouseDown, pnHand2.MouseDown, pnHand3.MouseDown, pnHand4.MouseDown, pnHand5.MouseDown, pnHand6.MouseDown _
            , pnAllyCard1.MouseDown, pnAllyCard2.MouseDown, pnAllyCard3.MouseDown, pnAllyCard4.MouseDown, pnAllyCard5.MouseDown, pnAllyCard6.MouseDown _
            , pnEnemyCard1.MouseDown, pnEnemyCard2.MouseDown, pnEnemyCard3.MouseDown, pnEnemyCard4.MouseDown, pnEnemyCard5.MouseDown, pnEnemyCard6.MouseDown
        Dim clickedPanel = TryCast(sender, Panel)

        '手札選択時
        If clickedPanel.Name.Substring(0, 6) = "pnHand" Then
            pnSelectPanelColor.Visible = False

            Dim clickNo = clickedPanel.Name.Substring(6, 1)
            Dim clickCardName = HandData(clickNo - 1).Name

            gbAllyInfo.Visible = True
            btDiscard.Visible = True

            Dim clickCardData = GetCardDataByName(clickCardName)
            SetAllySelectionData(clickCardData)

            SelectionCard = clickCardData
            SelectionCard.DeckNo = HandData(clickNo - 1).No

            Dim matchingControls = FindControlsRecursive(Controls, "btSetCard")

            For Each ctrl In matchingControls
                If BattleAreaAllyData(ctrl.Name.Substring(9, 1) - 1).Name <> "" Then
                    ctrl.Visible = False
                Else
                    ctrl.Visible = True
                End If
            Next

            pnSelectPanelColor.Left = clickedPanel.Location.X - 5
            pnSelectPanelColor.Top = clickedPanel.Location.Y - 5
            pnSelectPanelColor.Visible = True

            '味方配置選択時
        ElseIf clickedPanel.Name.Substring(0, 10) = "pnAllyCard" Then
            pnSelectPanelColor.Visible = False

            Dim clickNo = clickedPanel.Name.Substring(10, 1)
            If BattleAreaAllyData(clickNo - 1).Name = "" Then
                Dim matchingControls2 = FindControlsRecursive(Controls, "btSetCard")
                For Each ctrl In matchingControls2
                    ctrl.Visible = False
                Next
                btDiscard.Visible = False
                btReturnHand.Visible = False
                gbAllyInfo.Visible = False
                Exit Sub
            End If
            btDiscard.Visible = True
            btReturnHand.Visible = True
            gbAllyInfo.Visible = True
            Dim clickCardData = GetCardDataByName(BattleAreaAllyData(clickNo - 1).Name)
            SetAllySelectionData(clickCardData)
            SelectionCard = clickCardData
            SelectionCard.DeckNo = BattleAreaAllyData(clickNo - 1).DeckNo
            Dim matchingControls = FindControlsRecursive(Controls, "btSetCard")
            For Each ctrl In matchingControls
                If e.Button = MouseButtons.Right And ctrl.Name.Substring(9, 1) <> clickNo Then
                    ctrl.Visible = True
                Else
                    ctrl.Visible = False
                End If
            Next

            pnSelectPanelColor.Left = clickedPanel.Location.X - 5
            pnSelectPanelColor.Top = clickedPanel.Location.Y - 5
            pnSelectPanelColor.Visible = True

            '敵配置選択時
        ElseIf clickedPanel.Name.Substring(0, 11) = "pnEnemyCard" Then
            Dim clickNo = clickedPanel.Name.Substring(11, 1)
            If BattleAreaEnemyData(clickNo - 1).Name = "" Then
                Exit Sub
            End If
            gbEnemyInfo.Visible = True
            Dim clickCardData = GetCardDataByName(BattleAreaEnemyData(clickNo - 1).Name)
            SetEnemySelectionData(clickCardData)
            SelectionCardE = clickCardData
            SelectionCardE.DeckNo = BattleAreaEnemyData(clickNo - 1).DeckNo
            Dim matchingControls = FindControlsRecursive(Controls, "btSetCard")

            pnSelectPanelColorE.Left = clickedPanel.Location.X - 5
            pnSelectPanelColorE.Top = clickedPanel.Location.Y - 5
            pnSelectPanelColorE.Visible = True
        End If
    End Sub

    Private Sub btDiscard_Click(sender As Object, e As EventArgs) Handles btDiscard.Click
        Dim result As MsgBoxResult = MsgBox("選択中のカードを墓地に送りますか？", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "確認")
        If result = MsgBoxResult.No Then
            Exit Sub
        End If

        pnSelectPanelColor.Visible = False

        '手札から墓地へ送る場合
        For Each no As String In HandData.Select(Function(x) x.No).ToList()
            If no = SelectionCard.DeckNo Then
                Dim CardData = GetCardDataByName(SelectionCard.Name)
                CardData.DeckNo = SelectionCard.DeckNo
                GraveData.Add(CardData)
                coGrave.Items.Insert(0, SelectionCard.Name)
                coGrave.SelectedIndex = 0
                Dim resultList As IList(Of IList(Of Object)) =
                    GraveData.Select(Function(x) CType(New List(Of Object) From {x.DeckNo.ToString(), x.Name}, IList(Of Object))).ToList()
                Common.SetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_ARCHIVE, Constant.MAIN_SHEET_GRAVE_START_CELL(GetPlayerNo()), resultList)
                HandData.Remove(HandData.Find(Function(x) x.No = SelectionCard.DeckNo))
                HandCardUpdate()

                Dim matchingControls As List(Of Control) = FindControlsRecursive(Me.Controls, "btSetCard")

                For Each ctrl As Control In matchingControls
                    ctrl.Visible = False
                Next
                Exit For
            End If
        Next

        '味方配置から墓地へ送る場合
        For Each no As String In BattleAreaAllyData.Select(Function(x) x.DeckNo).ToList()
            If no = SelectionCard.DeckNo Then
                Dim CardData = GetCardDataByName(SelectionCard.Name)
                CardData.DeckNo = SelectionCard.DeckNo
                GraveData.Add(CardData)
                coGrave.Items.Insert(0, SelectionCard.Name)
                coGrave.SelectedIndex = 0
                Dim resultList As IList(Of IList(Of Object)) =
                    GraveData.Select(Function(x) CType(New List(Of Object) From {x.DeckNo.ToString(), x.Name}, IList(Of Object))).ToList()
                Common.SetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_ARCHIVE, Constant.MAIN_SHEET_GRAVE_START_CELL(GetPlayerNo()), resultList)
                Exit For
            End If
        Next

        For Each i As Integer In Enumerable.Range(0, BattleAreaAllyData.Count)
            If BattleAreaAllyData(i).DeckNo = SelectionCard.DeckNo Then
                BattleAreaAllyData(i) = New CardDataDto()
                BattleAllyUpdate()
                Exit For
            End If
        Next

        gbAllyInfo.Visible = False
        btDiscard.Visible = False
        lbHandMaxSign.Visible = False

        SetEnemyUpdateFlg()
    End Sub

    Private Sub btSetCard_Click(sender As Object, e As EventArgs) Handles btSetCard1.Click, btSetCard2.Click, btSetCard3.Click, btSetCard4.Click, btSetCard5.Click, btSetCard6.Click
        Dim result = MsgBox("ここへ配置しますか？", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "確認")
        If result = MsgBoxResult.No Then
            Exit Sub
        End If

        Dim clickedButton = TryCast(sender, Button)
        Dim clickNo = clickedButton.Name.Substring(9, 1)

        Dim foundpnAllyCard = FindControlsRecursive(Controls, "pnAllyCard" & clickNo)

        Dim targetpnAllyCard = foundpnAllyCard(0)
        Dim AllyCardPanel As Panel = targetpnAllyCard

        AllyCardPanel.Enabled = True

        pnSelectPanelColor.Visible = False
        pnSelectPanelColor.Left = AllyCardPanel.Location.X - 5
        pnSelectPanelColor.Top = AllyCardPanel.Location.Y - 5

        '手札から配置する場合
        For Each no In HandData.Select(Function(x) x.No).ToList
            If no = SelectionCard.DeckNo Then
                '配置処理
                Dim matchingControls = FindControlsRecursive(Controls, "btSetCard")
                For Each ctrl In matchingControls
                    ctrl.Visible = False
                Next
                HandData.Remove(HandData.Find(Function(x) x.No = SelectionCard.DeckNo))
                HandCardUpdate()
                BattleAreaAllyData(clickNo - 1) = SelectionCard
                lbHandMaxSign.Visible = False
                Exit For
            End If
        Next

        '味方配置から移動する場合
        Dim changeIndex = 0
        For Each no In BattleAreaAllyData.Select(Function(x) x.DeckNo).ToList
            If no = SelectionCard.DeckNo Then
                '配置処理
                Dim matchingControls = FindControlsRecursive(Controls, "btSetCard")
                For Each ctrl In matchingControls
                    ctrl.Visible = False
                Next
                If BattleAreaAllyData(clickNo - 1).DeckNo = "" Then
                    BattleAreaAllyData(clickNo - 1) = BattleAreaAllyData(changeIndex)
                    BattleAreaAllyData(changeIndex) = New CardDataDto

                    foundpnAllyCard = FindControlsRecursive(Controls, "pnAllyCard" & changeIndex + 1)

                    targetpnAllyCard = foundpnAllyCard(0)
                    AllyCardPanel = targetpnAllyCard

                    AllyCardPanel.Enabled = False
                Else
                    Dim tempCard = BattleAreaAllyData(clickNo - 1)
                    BattleAreaAllyData(clickNo - 1) = BattleAreaAllyData(changeIndex)
                    BattleAreaAllyData(changeIndex) = tempCard
                End If
                Exit For
            End If
            changeIndex += 1
        Next

        BattleAllyUpdate()

        pnSelectPanelColor.Visible = True
        btReturnHand.Visible = True

    End Sub

    Private Sub btReturnHand_Click(sender As Object, e As EventArgs) Handles btReturnHand.Click
        HandData.Add(New HandDataDto(SelectionCard.DeckNo, SelectionCard.Name, "1"))
        HandCardUpdate()

        For Each i In Enumerable.Range(0, BattleAreaAllyData.Count)
            If BattleAreaAllyData(i).DeckNo = SelectionCard.DeckNo Then
                BattleAreaAllyData(i) = New CardDataDto
                BattleAllyUpdate()
                pnSelectPanelColor.Visible = False
                btReturnHand.Visible = False
                Exit For
            End If
        Next

        BattleAllyUpdate()
    End Sub

    Private Async Function StartUpdateTask() As Task
        While con.FullState = System.Data.ConnectionState.Open
            Await DoUpdatesync()

            Await Task.Delay(Constant.UpdateDisplayMillSeconds)
        End While
    End Function

    ''' <summary>
    ''' 0.5秒感覚のディスプレイ更新
    ''' </summary>
    ''' <returns></returns>
    Private Async Function DoUpdatesync() As Task
        Await Task.Run(Sub()
                           Dim updataFlgData = DBManager.selectData(con, Query.selectCheckUpdateFlg(GetPlayerNo()))

                           Dim updateFlg = updataFlgData(0)("update_flg")

                           If updateFlg = "1" Then
                               Dim allData = Common.GetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_BATTLE, Constant.MAIN_SHEET_BATTLE_ALL_DATA)
                               If allData Is Nothing Then
                                   Return
                               End If
                               Me.Invoke(Sub() BattleEnemyUpdate(allData))

                               DBManager.exec(con, Query.updCheckUpdateFlg(GetPlayerNo(), 0))
                           End If
                       End Sub)
    End Function

    Private Sub SetAllySelectionData(selectCardData As CardDataDto)
        lbSelectCardName.Text = selectCardData.Cardname
        lbSelectCardCost.Text = selectCardData.Cost
        lbSelectCardDefenseMax.Text = "/" & selectCardData.Defense
        lbSelectCardIntellectMax.Text = "/" & selectCardData.Intellect
        txSelectCardDefense.Text = selectCardData.DefenseNow
        txSelectCardIntellect.Text = selectCardData.IntellectNow
        lbSelectCardAtack.Text = selectCardData.Atack
        lbSelectCardMagic.Text = selectCardData.Magic
        txSelectCarfAtackUpdate.Text = selectCardData.AttackUpdate
        txSelectCardMagicUpdate.Text = selectCardData.MagicUpdate
        lbSelectCardEffect.Text = selectCardData.Effect
        lbSelectCardLine.Text = selectCardData.Line
        lbSelectCardType.Text = selectCardData.Type

        lbSelectCardSkill1.Text = selectCardData.Skill3
        lbSelectCardSkill2.Text = selectCardData.Skill2
        lbSelectCardSkill3.Text = selectCardData.Skill3

        lbSelectCardFlavor.Text = selectCardData.Flavor
    End Sub

    Private Sub SetEnemySelectionData(selectCardData As CardDataDto)
        lbSelectCardNameE.Text = selectCardData.Cardname
        lbSelectCardCostE.Text = selectCardData.Cost
        lbSelectCardDefenseMaxE.Text = "/" & selectCardData.Defense
        lbSelectCardIntellectMaxE.Text = "/" & selectCardData.Intellect
        lbSelectCardDefenseE.Text = selectCardData.DefenseNow
        lbSelectCardIntellectE.Text = selectCardData.IntellectNow
        lbSelectCardAtackE.Text = selectCardData.Atack
        lbSelectCardMagicE.Text = selectCardData.Magic
        lbSelectCarfAtackUpdateE.Text = selectCardData.AttackUpdate
        lbSelectCardMagicUpdateE.Text = selectCardData.MagicUpdate
        lbSelectCardEffectE.Text = selectCardData.Effect
        lbSelectCardLineE.Text = selectCardData.Line
        lbSelectCardTypeE.Text = selectCardData.Type

        lbSelectCardSkill1.Text = selectCardData.Skill3
        lbSelectCardSkill2.Text = selectCardData.Skill2
        lbSelectCardSkill3.Text = selectCardData.Skill3

        lbSelectCardFlavor.Text = selectCardData.Flavor
    End Sub

    Private Sub HandCardUpdate()

        Dim handDataNameList = New List(Of String)

        For i As Integer = 1 To Constant.HandCardNum

            Dim foundlbHandName() As Control = Me.Controls.Find("lbHandName" & i.ToString(), True)

            Dim targetlbHandName As Control = foundlbHandName(0)
            Dim handLabel As Label = targetlbHandName

            Dim foundlbHandCost() As Control = Me.Controls.Find("lbHandCost" & i.ToString(), True)

            Dim targetlbHandCost As Control = foundlbHandCost(0)
            Dim handCostLabel As Label = targetlbHandCost

            Dim foundlbHandType() As Control = Me.Controls.Find("lbHandType" & i.ToString(), True)

            Dim targetlbHandType As Control = foundlbHandType(0)
            Dim handTypeLabel As Label = targetlbHandType

            Dim foundpnHand() As Control = Me.Controls.Find("pnHand" & i.ToString(), True)

            Dim targetpnHand As Control = foundpnHand(0)
            Dim handPanel As Panel = targetpnHand

            If (i - 1 < HandData.Count) Then

                handDataNameList.Add(HandData(i - 1).Name)

                Dim CardData = GetCardDataByName(HandData(i - 1).Name)
                handLabel.Text = CardData.Name
                handCostLabel.Text = CardData.Cost
                handTypeLabel.Text = CardData.Type
                handLabel.Visible = True
                handCostLabel.Visible = True
                handTypeLabel.Visible = True
                handPanel.Visible = True
            Else
                handDataNameList.Add("")
                handLabel.Visible = False
                handCostLabel.Visible = False
                handTypeLabel.Visible = False
                handPanel.Visible = False
            End If
        Next i

        Common.SetGoogleSheetDataAsync(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_BATTLE, Constant.MAIN_SHEET_BATTLE_HAND_START_CELL(GetPlayerNo()), {handDataNameList.ToArray()})

        SetEnemyUpdateFlg()
    End Sub

    Private Sub BattleAllyUpdate()

        For i As Integer = 1 To Constant.AllyCardNum

            Dim foundlbAllyCardName As List(Of Control) = FindControlsRecursive(Me.Controls, "lbAllyCardName" & i.ToString())

            Dim targetlbAllyCardName As Control = foundlbAllyCardName(0)
            Dim AllyCardNameLabel As Label = targetlbAllyCardName

            Dim foundtxAllyCardDefense As List(Of Control) = FindControlsRecursive(Me.Controls, "txAllyCardDefense" & i.ToString())

            Dim targettxAllyCardDefense As Control = foundtxAllyCardDefense(0)
            Dim AllyCardDefenseText As TextBox = targettxAllyCardDefense

            If BattleAreaAllyData(i - 1).Name <> "" Then
                Dim CardData = GetCardDataByName(BattleAreaAllyData(i - 1).Name)
                AllyCardNameLabel.Text = CardData.Name
                AllyCardDefenseText.Text = CardData.Defense
                AllyCardNameLabel.Visible = True
                AllyCardDefenseText.Visible = True
            Else
                AllyCardNameLabel.Visible = False
                AllyCardDefenseText.Visible = False
            End If
        Next i

        Common.SetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_BATTLE, Constant.MAIN_SHEET_BATTLE_AREA_START_CELL(GetPlayerNo()), {BattleAreaAllyData.Select(Function(x) x.Name).ToArray()})

        SetEnemyUpdateFlg()
    End Sub

    Private Sub BattleEnemyUpdate(allData As IList(Of IList(Of Object)))

        Dim enemyHandNameList = Common.GetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_BATTLE, Constant.MAIN_SHEET_BATTLE_ENWMY_HAND_RANGE(GetPlayerNo()))

        If enemyHandNameList IsNot Nothing Then
            If enemyHandNameList.Count = 0 Then
                Return
            End If
            Dim enemyHandCount = enemyHandNameList(0).Count

            For i As Integer = 1 To HandCardNum
                Dim foundpnEnemyHand As List(Of Control) = FindControlsRecursive(Me.Controls, "pnEnemyHand" & i.ToString())
                Dim EnemyHandPanel As Panel = foundpnEnemyHand(0)
                If i <= enemyHandCount Then
                    EnemyHandPanel.Visible = True
                Else
                    EnemyHandPanel.Visible = False
                End If
            Next
        End If

        ' 前回データをクリア
        BattleAreaEnemyData.Clear()

        Dim enemyNameList = Common.GetGoogleSheetData(Constant.MAIN_SHEET_ID, Constant.MAIN_SHEET_NAME_BATTLE, Constant.MAIN_SHEET_BATTLE_ENWMY_AREA_RANGE(GetPlayerNo()))

        If enemyNameList IsNot Nothing Then
            If enemyNameList.Count = 0 Then
                Return
            End If
            For Each name As String In enemyNameList(0)
                Dim CardData = GetCardDataByName(name.ToString())
                BattleAreaEnemyData.Add(CardData)
            Next
        End If

        For i As Integer = 1 To Constant.EnemyCardNum
            If BattleAreaEnemyData.Count < i Then
                BattleAreaEnemyData.Add(New CardDataDto())
            End If

            Dim foundlbEnemyCardName As List(Of Control) = FindControlsRecursive(Me.Controls, "lbEnemyCardName" & i.ToString())
            Dim targetlbEnemyCardName As Control = foundlbEnemyCardName(0)
            Dim EnemyCardNameLabel As Label = targetlbEnemyCardName

            Dim foundlbEnemyCardDefense As List(Of Control) = FindControlsRecursive(Me.Controls, "lbEnemyCardDefense" & i.ToString())
            Dim targetlbEnemyCardDefense As Control = foundlbEnemyCardDefense(0)
            Dim EnemyCardDefenseLabel As Control = targetlbEnemyCardDefense

            If BattleAreaEnemyData(i - 1).Name <> "" Then
                Dim CardData = GetCardDataByName(BattleAreaEnemyData(i - 1).Name)
                EnemyCardNameLabel.Text = CardData.Name
                EnemyCardDefenseLabel.Text = CardData.Defense
                EnemyCardNameLabel.Visible = True
                EnemyCardDefenseLabel.Visible = True
            Else
                EnemyCardNameLabel.Visible = False
                EnemyCardDefenseLabel.Visible = False
            End If
        Next i
    End Sub

    Private Sub SetEnemyUpdateFlg()
        DBManager.exec(con, Query.updCheckUpdateFlg(ENEMY_NO(GetPlayerNo()), 1))
    End Sub

    Private Function FindControlsRecursive(ByVal rootControls As Control.ControlCollection, ByVal searchPart As String) As List(Of Control)
        Dim foundControls As New List(Of Control)()

        For Each ctrl As Control In rootControls

            If ctrl.Name.IndexOf(searchPart, StringComparison.OrdinalIgnoreCase) >= 0 Then
                foundControls.Add(ctrl)
            End If

            If ctrl.HasChildren Then
                foundControls.AddRange(FindControlsRecursive(ctrl.Controls, searchPart))
            End If
        Next

        Return foundControls
    End Function

    ''' <summary>
    ''' 指定されたフォルダ内で '.login' ファイルを検索し、存在する場合はそのファイル名のみを返します。
    ''' 存在しない場合は、空の文字列を返します。
    ''' </summary>
    ''' <param name="searchDirectory">検索対象のフォルダパス。</param>
    ''' <returns>見つかった '.login' ファイルのファイル名、または空の文字列。</returns>
    Function FindLoginFile() As String
        Try
            Const searchPattern As String = "*.login"

            Dim loginFiles() As String = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, searchPattern)

            ' ファイルが見つかったか確認
            If loginFiles.Length > 0 Then
                Dim fullPath As String = loginFiles(0)

                Dim fileNameWithExt As String = Path.GetFileName(fullPath)

                Return Path.GetFileNameWithoutExtension(fileNameWithExt)
            Else
                Return String.Empty
            End If

        Catch ex As DirectoryNotFoundException
            Return String.Empty
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        con.Close()
    End Sub
End Class
