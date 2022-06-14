namespace DomainServices.SalesArrangementService.Api.Handlers.SalesArrangement
{
    public class FormDataBuilder
    {
        private static int[] FormIDs = new int[] { 3601001 };

        #region SampleFormData

        public static Eas.EasWrapper.CheckFormData BuildSampleFormData(int formId = 3601001)
        {
            if (!FormIDs.Contains(formId))
            {
                throw new CisArgumentException(99999, $"FormId #{formId} is not supported.", nameof(formId));  //TODO: ErrorCode
            }

            Eas.EasWrapper.CheckFormData formData = null;

            switch (formId)
            {
                case 3601001:
                    formData = BuildSampleFormData_3601001();
                    break;
            }

            return formData!;
        }

        private static Eas.EasWrapper.CheckFormData BuildSampleFormData_3601001()
        {
            var sampleData = "{\"cislo_dokumentu\":\"9876543210\",\"cislo_smlouvy\":\"1234567890\",\"datum_spisania\":\"01.11.2021\",\"CPM\":\"123456\",\"ICP\":\"654321\",\"formular_kod_ex\":\"2\",\"seznam_ucastniku\":[{\"klient\":{\"rodne_cislo_ico\":\"1234569024\",\"titul_pred\":\"Ing.\",\"prijmeni_nazev\":\"Testovic\",\"jmeno\":\"Test\",\"titul_za\":\"\"},\"seznam_adres\":[{\"typ_adresy\":\"trvala\",\"ulice\":\"Testovacia\",\"cislo_popisne\":\"32\",\"cislo_orientacni\":\"21\",\"ulice_dodatek\":\"\",\"psc\":\"60012\",\"misto\":\"Brno\",\"stat\":\"\"}]},\"role\":\"vlastnik\"}]}";
            sampleData = "{}";
            sampleData = "{cislo_dokumentu:\"9876543210\", cislo_smlouvy: \"1234567890\", datum_spisania: \"01.11.2021\" }";

            var formData = new Eas.EasWrapper.CheckFormData()
            {
                formular_id = 3601001,
                cislo_smlouvy = "1234567890",
                dokument_id = "9876543210",
                datum_prijeti = new DateTime(2022, 1, 1),
                data = sampleData
            };

            return formData;
        }

        #endregion

    }
}
