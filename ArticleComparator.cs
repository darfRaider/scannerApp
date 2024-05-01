using scanapp.Models;

namespace scanapp {

    class ArticleComparer : IEqualityComparer<Article>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(Article x, Article y)
        {

            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the products' properties are equal.
            return x.ArticleName == y.ArticleName;
        }

        // If Equals() returns true for a pair of objects
        // then GetHashCode() must return the same value for these objects.

        public int GetHashCode(Article article)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(article, null)) return 0;

            //Get hash code for the Name field if it is not null.
            return article.ArticleName.GetHashCode();
        }
    }

}