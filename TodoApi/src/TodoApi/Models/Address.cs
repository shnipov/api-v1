namespace TodoApi.Models
{
    /// <summary>
    /// �����
    /// </summary>
    public class Address
    {
        /// <summary>
        /// ������
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// �����
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// �����
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// ����� ������
        /// </summary>
        public string CityDistrict { get; set; }

        /// <summary>
        /// ��������� �����
        /// </summary>
        public string Settlement { get; set; }

        /// <summary>
        /// �����
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// ���
        /// </summary>
        public string House { get; set; }

        /// <summary>
        /// ����� ���������� ����
        /// </summary>
        public string Block { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public string Flat { get; set; }
    }
}